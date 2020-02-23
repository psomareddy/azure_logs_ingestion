# Azure EventHub integration for New Relic Logs

[New Relic Logs](https://docs.newrelic.com/docs/introduction-new-relic-logs) offers an integration to forward log messages from  [Azure EventHub](https://azure.microsoft.com/en-us/services/event-hubs/) to New Relic Logs. This allows you to collect logs from all your Azure services (except App services) and view them in New Relic Logs.

## Disclaimer

New Relic has open-sourced this integration to enable monitoring of this technology. This integration is provided AS-IS WITHOUT WARRANTY OR SUPPORT, although you can report issues and contribute to this integration via GitHub. Support for this integration is available with an [Expert Services subscription](https://newrelic.com/expertservices).

## Requirements

To use New Relic Logs with the New Relic Logs Azure EventHub integration, ensure your configuration meets the following requirements:
- [New Relic license key](https://docs.newrelic.com/docs/accounts/install-new-relic/account-setup/license-key)

## Create Azure Event Hub

- Create an Azure Event Hub
- Create `newrelic_logs_consumer_group` consumer group in the event hub for use by New Relic Logs Azure Function
- Configure your Azure services to forward their logs to the Event Hub

## Install the New Relic Logs Azure Function 

Edit Constants.cs file and update the following properties
- EVENT_HUB_NAME
- CONSUMER_GROUP

Publish the function app.

## Configure the Azure Function

Edit New Relic Logs Azure Function's App Service Settings to ensure valid values for these properties

- EVENT_HUB_CONNECTION_STRING

- NR_LICENSE_KEY

- NR_LOGGING_ENDPOINT

## View log data

If everything is configured correctly and your data is being collected, you should see data logs in the  [New Relic Logs UI](https://one.newrelic.com/launcher/logger.log-launcher "Link opens in a new window.") or by going to  [Insights](https://insights.newrelic.com/ "Link opens in a new window.") and querying:

```cmd
SELECT * FROM Log
```

## What's next?
[Query your data](https://docs.newrelic.com/docs/logs/new-relic-logs/ui-data/query-syntax-logs) and [create custom dashboards](https://docs.newrelic.com/docs/insights/use-insights-ui/manage-dashboards/create-edit-copy-insights-dashboards), [charts](https://docs.newrelic.com/docs/insights/use-insights-ui/manage-dashboards/add-customize-nrql-charts), or [alerts](https://docs.newrelic.com/docs/alerts/new-relic-alerts/configuring-alert-policies/create-edit-or-find-alert-policy).