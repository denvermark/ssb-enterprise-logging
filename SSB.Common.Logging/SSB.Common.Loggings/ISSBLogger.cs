using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSB.Common.Logging
{
    public interface ISsbLogger
    {
        Task Log(LogEntry entry);
    }
}
