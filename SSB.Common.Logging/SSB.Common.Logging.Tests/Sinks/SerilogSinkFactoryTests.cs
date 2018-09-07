using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Core;
using Ssb.Common.Logging.Sinks.Serilog;

namespace SSB.Common.Logging.Tests.Sinks
{
    [TestClass]
    public class SerilogSinkFactoryTests
    {
        [TestMethod]
        public void TestSerilogSinkFactoryLoadsEventGridSinkWithConfig()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var factory = new SerilogSinkFactory();

            var sinkConfig = new KeyValuePair<string, Dictionary<string, string>>(
                "Serilog.AzureEventGrid",
                new Dictionary<string, string>
                {
                    {"EventGridTopicKey", "testKey"},
                    {"EventGridTopicUri", "http://testUri.com"}
                });

            // act
            var sink = factory.GetSink(sinkConfig, direction, integration, batchId, instanceId);

            // assert
            Assert.IsInstanceOfType(sink, typeof(SerilogEventGridSink));
        }

        [TestMethod]
        public void TestSerilogSinkFactoryErrorsWithInvalidUri()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var factory = new SerilogSinkFactory();

            var sinkConfig = new KeyValuePair<string, Dictionary<string, string>>(
                "Serilog.AzureEventGrid",
                new Dictionary<string, string>
                {
                    {"EventGridTopicKey", "testKey"},
                    {"EventGridTopicUri", "invalidUri"}
                });

            try
            {
                // act
                factory.GetSink(sinkConfig, direction, integration, batchId, instanceId);

                // assert
                Assert.Fail("A malformed uri was allowed.");
            }
            catch (UriFormatException goodException) { }
        }
    }
}
