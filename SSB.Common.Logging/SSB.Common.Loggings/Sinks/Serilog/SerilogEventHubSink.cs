using System.Configuration;
using Collector.Serilog.Sinks.AzureEventHub;
using Microsoft.ServiceBus.Messaging;
using Serilog.Core;
using Serilog.Events;
using Ssb.Common.Logging.Exceptions;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    internal class SerilogEventHubSink : ILogEventSink
    {
        private readonly ILogEventSink _sink;

        public SerilogEventHubSink()
        {
            // get the event hub connectionstring from the config file
            var connectionString = ConfigurationManager.ConnectionStrings["Serilog.EventHub"].ConnectionString;

            // ensure the connectionstring exists
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new LogException("Event Hub ConnectionString is null or empty."); //TODO: put in resource file or similar
            }

            // create the Event Hub client
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, "loggingevents");

            // create the Event Hub serilog sink
            var eventHubSink = new AzureEventHubSink(eventHubClient);

            _sink = eventHubSink;
        }

        public void Emit(LogEvent logEvent)
        {
            _sink.Emit(logEvent);
        }
    }


}