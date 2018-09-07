using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collector.Serilog.Sinks.AzureEventHub;
using Microsoft.ServiceBus.Messaging;
using Serilog.Core;
using Serilog.Sinks.EventGrid;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    public class SerilogSinkFactory
    {
        private readonly string _direction;
        private readonly string _integration;
        private readonly string _batchId;
        private readonly string _instanceId;

        public SerilogSinkFactory(string direction, string integration, string batchId, string instanceId)
        {
            _direction = direction;
            _integration = integration;
            _batchId = batchId;
            _instanceId = instanceId;
        }

        public virtual ILogEventSink GetSink(KeyValuePair<string, Dictionary<string, string>> sinkConfig)
        {
            ILogEventSink sink = null;

            switch (sinkConfig.Key)
            {
                case "Serilog.AzureEventGrid":
                    sink = GetEventGridSink(sinkConfig.Value);
                    break;
                case "Serilog.AzureEventHub":
                    sink = GetEventHubSink(sinkConfig.Value);
                    break;
            }

            return sink;
        }

        private SerilogEventHubSink GetEventHubSink(Dictionary<string, string> config)
        {
            dynamic eventHubConfig = SerilogEventHubSink.GetConfig(config);

            // create the Event Hub client
            var eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConfig.ConnectionString, eventHubConfig.ContainerName);

            // create the Event Hub serilog sink
            var sink = new AzureEventHubSink(eventHubClient);

            var eventHubSink = new SerilogEventHubSink(sink);

            return eventHubSink;
        }

        private SerilogEventGridSink GetEventGridSink(Dictionary<string, string> dict)
        {
            dynamic eventGridConfig = SerilogEventGridSink.GetConfig(dict);

            var sink = new EventGridSink(
                CultureInfo.CurrentCulture,
                eventGridConfig.TopicKey,
                new Uri(eventGridConfig.TopicUri),
                "",
                "",
                CustomEventRequestAuth.Key,
                "",
                "",
                10,
                new TimeSpan(0, 0, 15));

            var eventGridSink = new SerilogEventGridSink(eventGridConfig.TopicUri, sink, _direction, _integration, _batchId, _instanceId);

            return eventGridSink;
        }
    }
}
