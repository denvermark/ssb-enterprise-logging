using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ssb.Common.Logging.Adapters;

namespace SSB.Common.Logging.Tests
{
    /// <summary>
    /// Summary description for Logger
    /// </summary>
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void TestLoggerCallsAdapter()
        {
            // arrange
            var direction = "testDirection";
            var integration = "testIntegration";
            var batchId = "testBatchId";
            var instanceId = "testInstanceId";

            var logAdapter = new Mock<ILoggerAdapter>();
            logAdapter
                .Setup(x => x.Debug(It.Is<string>(m => m == "testMessage")));

            var adapterFactory = new Mock<LoggerAdapterBase.Factory>();
            adapterFactory
                .Setup(x => x(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(logAdapter.Object);
            //adapterFactory
            //    .Setup(x => x.Invoke(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(logAdapter.Object);

            var logger = new SsbLogger(adapterFactory.Object, direction, integration, batchId, instanceId);

            // act
            logger.Debug("testMessage");

            // assert
            logAdapter.VerifyAll();
        }
    }
}
