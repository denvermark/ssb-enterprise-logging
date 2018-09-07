using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSB.Common.Logging
{
    public class SsbLogger: ISsbLogger
    {
        private readonly ILogAdapter _logAdapter;

        public SsbLogger(ILogAdapter logAdapter)
        {
            this._logAdapter = logAdapter;
        }

        public async Task Log(LogEntry entry)
        {
            await _logAdapter.Log(entry);
        }
    }
}
