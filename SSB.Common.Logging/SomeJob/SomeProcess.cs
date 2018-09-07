using Autofac;
using SSB.Common.Logging;

namespace SomeJob
{
    public class SomeProcess : ISomeProcess
    {
        private readonly ISomeOtherProcess _someOtherProcess;
        private readonly ISsbLogger _logger;

        public SomeProcess(ISsbLogger loggerFactory, ISomeOtherProcess someOtherProcess)
        {
            _someOtherProcess = someOtherProcess;

            _logger = loggerFactory; // loggerFactory(direction, integration, batchId, instanceId);
        }

        public void Run()
        {
            var jobId = "12345";
            _logger.Information($"Created job: {jobId}");

            _someOtherProcess.DoSomething();
        }
    }
}
