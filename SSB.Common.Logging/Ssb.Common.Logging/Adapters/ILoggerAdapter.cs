using System;

namespace Ssb.Common.Logging.Adapters
{
    public interface ILoggerAdapter
    {
        void Debug(string message);

        void Information(string message);

        void Warning(string message);

        void Error(string message, Exception ex);

        void Fatal(string message, Exception ex);
    }
}