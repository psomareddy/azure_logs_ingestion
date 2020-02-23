using System;
using System.Collections.Generic;
using System.Text;

namespace NewRelic.Azure
{
    public class Constants
    {
        // Provide the name of the Event Hub to subscribe for log messages
        public const string EVENT_HUB_NAME = "first-event-hub";

        // Please do not specify "$Default" or any consumer group name used by other applications
        public const string CONSUMER_GROUP = "newrelic_logs_consumer_group";

        //Do not edit this property
        //The App Settings key that will specify the connection string (and not the actual connection string itself).
        public const string EVENT_HUB_CONNECTION_STRING = "EVENT_HUB_CONNECTION_STRING";

    }
}
