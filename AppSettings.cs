using System;
using System.Collections.Generic;
using System.Text;

namespace NewRelic.Azure
{
    public class AppSettings
    {
        public string LoggingEndpoint { get; set; }

        public string LicenseKey { get; set; }

        public string ApiKey { get; set; }
    }
}
