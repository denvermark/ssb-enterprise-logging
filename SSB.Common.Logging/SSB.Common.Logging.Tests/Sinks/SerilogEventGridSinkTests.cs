using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.EventGrid;
using Ssb.Common.Logging.Adapters;
using Ssb.Common.Logging.Sinks.Serilog;

namespace SSB.Common.Logging.Tests.Sinks
{
    [TestClass]
    public class SerilogEventGridSinkTests
    {

        [TestMethod]
        public void TestSerilogAdapterLoadsEventGridSink()
        {
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            // arrange
            var sink = new Mock<ILogEventSink>(MockBehavior.Strict);
            sink.Setup(x => x.Emit(It.IsAny<LogEvent>()));
            
            var entry = new LogEvent(
                new DateTimeOffset(new DateTime(3, 2, 1)),
                LogEventLevel.Debug,
                null,
                new MessageTemplate(new List<MessageTemplateToken>()
                {
                    new TextToken("testTemplate")
                }),
                new List<LogEventProperty>());

            var eventGridSink = new SerilogEventGridSink("testUri", sink.Object, direction, integration, batchId, instanceId);
            
            // act
            eventGridSink.Emit(entry);

            // assert
            sink.Verify(x => x.Emit(It.Is<LogEvent>(m => m.MessageTemplate.Text.Contains("testTemplate"))));
            sink.Verify(x => x.Emit(It.Is<LogEvent>(t => t.Timestamp.Year == 3)));
            sink.Verify(x => x.Emit(It.Is<LogEvent>(t => t.Timestamp.Month == 2)));
            sink.Verify(x => x.Emit(It.Is<LogEvent>(t => t.Timestamp.Day == 1)));
        }
    }
}
