using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace NewRelic.Azure
{
    public class Payload
    {
        [JsonPropertyName("common")]
        public CommonAttributes Common { get; set; } = new CommonAttributes();

        [JsonPropertyName("logs")]
        public List<LogItem> Logs { get; } = new List<LogItem>();

        public void AddCommonAttribute(string key, string value)
        {
            Common.AddAttribute(key, value);
        }
        public void AddLogItem(LogItem logItem)
        {
            Logs.Add(logItem);
        }
    }

    public class CommonAttributes
    {
        [JsonPropertyName("attributes")]
        public Dictionary<string, string> attributes { get; } = new Dictionary<string, string>();

        public void AddAttribute(string key, string value)
        {
            attributes.Add(key, value);
        }
    }

    public class LogItem
    {
        [JsonPropertyName("timestamp")]
        public long timestamp { get; set; }

        [JsonPropertyName("message")]
        public string message { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, string> attributes { get; } = new Dictionary<string, string>();

        public void AddAttribute(string key, string value)
        {
            attributes.Add(key, value);
        }
    }

}
