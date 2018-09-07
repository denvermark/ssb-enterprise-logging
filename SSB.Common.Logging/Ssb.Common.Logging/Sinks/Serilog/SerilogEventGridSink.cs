using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.EventGrid;

namespace Ssb.Common.Logging.Sinks.Serilog
{
    public class SerilogEventGridSink : ILogEventSink
    {
        private readonly ILogEventSink _sink;
        private readonly string _direction;
        private readonly string _integration;
        private readonly string _batchId;
        private readonly string _instanceId;
        private string _topicUri;
        private const string EventSubjectBase = "integration/cidt";
        private const string EventVersion = "v1";

        //// Factory method
        //public delegate SerilogEventGridSink Factory(ILogEventSink sink, string direction, string integration, string batchId, string instanceId);

        public SerilogEventGridSink(string topicUri, ILogEventSink sink, string direction, string integration, string batchId, string instanceId)
        {
            _sink = sink;
            _direction = direction;
            _integration = integration;
            _batchId = batchId;
            _instanceId = instanceId;
            _topicUri = topicUri;
            _topicUri = topicUri;
        }

        public static object GetConfig(Dictionary<string, string> eventGridSettings)
        {
            return new
            {
                TopicKey = eventGridSettings["EventGridTopicKey"],
                TopicUri = eventGridSettings["EventGridTopicUri"]
            };
        }
        
        public virtual void Emit(LogEvent logEvent)
        {
            // add event grid wrapper to message
            var messageJson = new
            {
                Topic = _topicUri,
                Subject = $"{EventSubjectBase}/{_direction}/{_integration}/{EventVersion}",
                EventType = $"SSB.{logEvent.Level.ToString()}",
                Data = new
                {
                    BatchId = _batchId,
                    InstanceId = _instanceId,
                    Message = logEvent.RenderMessage(),
                    Timestamp = DateTime.Now.ToString(CultureInfo.CurrentCulture)
                }
            };

            var formattedLogEvent = new LogEvent(
                logEvent.Timestamp,
                logEvent.Level,
                logEvent.Exception,
                new MessageTemplate(new List<MessageTemplateToken>()
                {
                    new TextToken(JsonConvert.SerializeObject(messageJson))
                }),
                new List<LogEventProperty>());

            _sink.Emit(formattedLogEvent);
        }
    }
}