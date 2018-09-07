using System.Collections.Generic;
using Collector.Serilog.Sinks.AzureEventHub;
using Serilog.Core;
using Serilog.Events;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    internal class SerilogEventHubSink : ILogEventSink
    {
        private readonly ILogEventSink _sink;

        public SerilogEventHubSink(AzureEventHubSink sink)
        {

            _sink = sink;
        }

        public static object GetConfig(Dictionary<string, string> eventHubConfig)
        {
            return new
            {
                ConnectionString = eventHubConfig["ConnectionString"],
                ContainerName = eventHubConfig["ContainerName"]
            };
        }

        public void Emit(LogEvent logEvent)
        {
            // configure logEvent for Event Hub specific format
            _sink.Emit(logEvent);
        }
    }


}