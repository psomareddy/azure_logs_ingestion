using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Threading;

namespace NewRelic.Azure
{
    public class LoggingClient
    {
        // _httpClient isn't exposed publicly
        private readonly HttpClient _client;

        private readonly AppSettings _settings;

        public LoggingClient(IOptions<AppSettings> options, HttpClient client)
        {
            _settings = options.Value;
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri(_settings.LoggingEndpoint);
            
            
            if (String.IsNullOrEmpty(_settings.LicenseKey))
            {
               if (String.IsNullOrEmpty(_settings.ApiKey))
                {
                    throw new Exception("LICENSE_KEY app setting must not be null or empty");
                } else
                {
                    _client.DefaultRequestHeaders.Add("X-Insert-Key", _settings.ApiKey);
                }
            } else
            {
                _client.DefaultRequestHeaders.Add("X-License-Key", _settings.LicenseKey);
            }
        }

        public async Task PostStreamAsync(Payload content, CancellationToken cancellationToken)
        {
            using (var httpContent = CreateHttpContent(content))
            {
                var response = await _client.PostAsync("", httpContent);
                response.EnsureSuccessStatusCode();
            }
        }

        private static HttpContent CreateHttpContent(Payload content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        private static void SerializeJsonIntoStream(Payload payload, Stream stream)
        {
            var options = new JsonWriterOptions
            {
                Indented = true
            };

            using (var writer = new Utf8JsonWriter(stream, options))
            {
                writer.WriteStartArray(); // start payload array
                writer.WriteStartObject(); // start payload[0] object

                //common section
                writer.WritePropertyName("common");
                writer.WriteStartObject(); // start common object
                writer.WritePropertyName("attributes");
                writer.WriteStartObject(); // start attributes value
                //writer.WritePropertyName("key");
                //writer.WriteStringValue("value");
                writer.WriteEndObject(); // end attributes value
                writer.WriteEndObject(); // end common object

                writer.WritePropertyName("logs");
                writer.WriteStartArray(); //start logs property's value array
                //write each log object
                foreach (var logItem in payload.Logs)
                {
                    writer.WriteStartObject(); // start log item object
                    //writer.WriteNumber("timestamp", DateTime.Now.Ticks);
                    writer.WritePropertyName("message");
                    writer.WriteStringValue(logItem.message);
                    writer.WritePropertyName("attributes");
                    writer.WriteStartObject(); // start attributes value
                    foreach (KeyValuePair<string, string> entry in logItem.attributes)
                    {
                        writer.WritePropertyName(entry.Key);
                        writer.WriteStringValue(entry.Value);
                    }
                    writer.WriteEndObject(); // end attributes value
                    writer.WriteEndObject(); // end log item object
                }
                writer.WriteEndArray(); //end logs property's value array

                writer.WriteEndObject(); // end payload[0] object
                writer.WriteEndArray(); // end payload array
                writer.Flush(); 
            }
        }
    }
}
