using System;
using SSB.Common.Logging;

namespace SomeJob
{
    public class SomeOtherProcess : ISomeOtherProcess
    {
        private readonly ISsbLogger _logger;

        public SomeOtherProcess(ISsbLogger logger)
        {
            _logger = logger;
        }

        public void DoSomething()
        {
            _logger.Error("Error message", new Exception());
        }
    }
}