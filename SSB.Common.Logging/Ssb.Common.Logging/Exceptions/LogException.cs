using System;

namespace Ssb.Common.Logging.Exceptions
{
    public class LogException: Exception
    {
        public LogException()
        {
        }

        public LogException(string message)
            : base(message)
        {
        }

        public LogException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
