using Ssb.Common.Logging.Adapters;
using System;

namespace SSB.Common.Logging
{
    public class SsbLogger: ISsbLogger
    {
        private readonly ILoggerAdapter _logAdapter;

        //// Factory method
        //public delegate SsbLogger Factory(string direction, string integration, string batchId, string instanceId);
        
        public SsbLogger(ILoggerAdapter adapter)
        {
            _logAdapter = adapter; //Factory(direction, integration, batchId, instanceId);
        }

        public void Debug(string message)
        {
            _logAdapter.Debug(message);
        }

        public void Error(string message, Exception ex)
        {
            _logAdapter.Error(message, ex);
        }

        public void Fatal(string message, Exception ex)
        {
           _logAdapter.Fatal(message, ex);
        }

        public void Information(string message)
        {
            _logAdapter.Information(message);
        }

        public void Warning(string message)
        {
            _logAdapter.Warning(message);
        }
    }
}
