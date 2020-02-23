using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace NewRelic.Azure
{
    public class EventHubTrigger
    {
        private readonly LoggingClient _newrelicLogService;

        public EventHubTrigger(LoggingClient newrelicLogService)
        {
            _newrelicLogService = newrelicLogService ?? throw new ArgumentNullException(nameof(newrelicLogService));
        }

        [FunctionName("LogConsumerFunc")]
        public async Task Run([EventHubTrigger(Constants.EVENT_HUB_NAME, Connection = Constants.EVENT_HUB_CONNECTION_STRING, ConsumerGroup = Constants.CONSUMER_GROUP)] EventData[] events, ILogger log, ExecutionContext context)
        {
            var exceptions = new List<Exception>();
            Payload payload = new Payload();
            foreach (EventData eventData in events)
            {
                try
                {
                    LogItem logItem = new LogItem();
                    ArraySegment<byte> b = eventData.Body;
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                   
                    logItem.message = messageBody;
                    logItem.timestamp = DateTime.Now.Ticks;
                    foreach (KeyValuePair<string, object> entry in eventData.Properties)
                    {
                        logItem.AddAttribute(entry.Key, entry.Value.ToString());
                    }
                    payload.AddLogItem(logItem);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            try
            {
               await _newrelicLogService.PostStreamAsync(payload, System.Threading.CancellationToken.None); 
            } catch(Exception e)
            {
                log.LogError(e.Message);
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
        
    }

}
