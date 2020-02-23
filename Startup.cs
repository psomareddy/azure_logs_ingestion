using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(NewRelic.Azure.Startup))]

namespace NewRelic.Azure
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<AppSettings>()
                .Configure<IConfiguration>((settings, configuration) => {
                    settings.LicenseKey = configuration.GetValue("NR_LICENSE_KEY", "NR_LICENSE_KEY");
                    settings.LoggingEndpoint = configuration.GetValue("NR_LOGGING_ENDPOINT", "NR_LOGGING_ENDPOINT");
                });

            builder.Services.AddHttpClient<LoggingClient>();
        }
    }
}
