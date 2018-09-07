using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Serilog.Sinks.EventGrid;
using Ssb.Common.Logging.Adapters;
using Ssb.Common.Logging.Exceptions;
using Ssb.Common.Logging.Sinks.Serilog;

namespace SSB.Common.Logging.Tests.Adapters
{
    [TestClass]
    public class SerilogAdapterTests
    {
        private readonly Mock<SerilogSinkFactory> _sinkFactory;

        public SerilogAdapterTests()
        {

            _sinkFactory = new Mock<SerilogSinkFactory>();
        }

        [TestMethod]
        public void TestAdapterCallsSinkFromErrorLog()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var sink = new Mock<ILogEventSink>(MockBehavior.Strict);
            sink
                .Setup(x => x.Emit(It.IsAny<LogEvent>()));

            _sinkFactory
                .Setup(x => x.GetSink(It.IsAny<KeyValuePair<string, Dictionary<string, string>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sink.Object);

            var loggerConfig = new LoggerConfiguration();

            var loggingConfig = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Serilog.AzureEventGrid",
                    new Dictionary<string, string>
                    {
                        {"ConnectionString", ""},{"ContainerName", ""}
                    }
                }
            };

            var adapter = new SerilogAdapter(loggerConfig, _sinkFactory.Object, loggingConfig, direction, integration, batchId, instanceId);

            // act
            adapter.Error("testMessage", new Exception("testException"));

            // assert
            sink.Verify(x => x.Emit(It.Is<LogEvent>(m => m.MessageTemplate.Text == "testMessage")));
            sink.Verify(x => x.Emit(It.Is<LogEvent>(ex => ex.Exception.Message == "testException")));
        }

        [TestMethod]
        public void TestAdapterCallsSinkFromDebugLog()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var sink = new Mock<ILogEventSink>(MockBehavior.Strict);
            sink.Setup(x => x.Emit(It.IsAny<LogEvent>()));

            _sinkFactory
                .Setup(x => x.GetSink(It.IsAny<KeyValuePair<string, Dictionary<string, string>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sink.Object);

            var loggerConfig = new LoggerConfiguration();
            loggerConfig.MinimumLevel.ControlledBy(new LoggingLevelSwitch(LogEventLevel.Debug));

            var loggingConfig = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Serilog.AzureEventGrid",
                    new Dictionary<string, string>
                    {
                        {"ConnectionString", ""},{"ContainerName", ""}
                    }
                }
            };

            var adapter = new SerilogAdapter(loggerConfig, _sinkFactory.Object, loggingConfig, direction, integration, batchId, instanceId);

            // act
            adapter.Debug("testMessage");

            // assert
            sink.Verify(x => x.Emit(It.Is<LogEvent>(m => m.MessageTemplate.Text == "testMessage")));
        }

        [TestMethod]
        public void TestAdapterCallsSinkFromFatalLog()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var sink = new Mock<ILogEventSink>(MockBehavior.Strict);
            sink
                .Setup(x => x.Emit(It.IsAny<LogEvent>()));

            _sinkFactory
                .Setup(x => x.GetSink(It.IsAny<KeyValuePair<string, Dictionary<string, string>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sink.Object);

            var loggerConfig = new LoggerConfiguration();
            var loggingConfig = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Serilog.AzureEventGrid",
                    new Dictionary<string, string>
                    {
                        {"ConnectionString", ""},{"ContainerName", ""}
                    }
                }
            };

            var adapter = new SerilogAdapter(loggerConfig, _sinkFactory.Object, loggingConfig, direction, integration, batchId, instanceId);

            // act
            adapter.Fatal("testMessage", new Exception("testException"));

            // assert
            sink.Verify(x => x.Emit(It.Is<LogEvent>(m => m.MessageTemplate.Text == "testMessage")));
            sink.Verify(x => x.Emit(It.Is<LogEvent>(ex => ex.Exception.Message == "testException")));
        }

        [TestMethod]
        public void TestAdapterCallsSinkFromWarningLog()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var sink = new Mock<ILogEventSink>(MockBehavior.Strict);
            sink
                .Setup(x => x.Emit(It.IsAny<LogEvent>()));

            _sinkFactory
                .Setup(x => x.GetSink(It.IsAny<KeyValuePair<string, Dictionary<string, string>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sink.Object);

            var loggerConfig = new LoggerConfiguration();
            var loggingConfig = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Serilog.AzureEventGrid",
                    new Dictionary<string, string>
                    {
                        {"ConnectionString", ""},{"ContainerName", ""}
                    }
                }
            };

            var adapter = new SerilogAdapter(loggerConfig, _sinkFactory.Object, loggingConfig, direction, integration, batchId, instanceId);

            // act
            adapter.Warning("testMessage");

            // assert
            sink.Verify(x => x.Emit(It.Is<LogEvent>(m => m.MessageTemplate.Text == "testMessage")));
        }

        [TestMethod]
        public void TestAdapterCallsSinkFromInformationLog()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var sink = new Mock<ILogEventSink>(MockBehavior.Strict);
            sink
                .Setup(x => x.Emit(It.IsAny<LogEvent>()));

            _sinkFactory
                .Setup(x => x.GetSink(It.IsAny<KeyValuePair<string, Dictionary<string, string>>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(sink.Object);

            var loggerConfig = new LoggerConfiguration();
            var loggingConfig = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "Serilog.AzureEventGrid",
                    new Dictionary<string, string>
                    {
                        {"ConnectionString", ""},{"ContainerName", ""}
                    }
                }
            };

            var adapter = new SerilogAdapter(loggerConfig, _sinkFactory.Object, loggingConfig, direction, integration, batchId, instanceId);

            // act
            adapter.Information("testMessage");

            // assert
            sink.Verify(x => x.Emit(It.Is<LogEvent>(m => m.MessageTemplate.Text == "testMessage")));
        }
    }
}
