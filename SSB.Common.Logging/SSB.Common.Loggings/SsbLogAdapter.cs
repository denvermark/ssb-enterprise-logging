using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SSB.Common.Logging
{
    public class SsbLogAdapter: ILogAdapter
    {
        public async Task Log(LogEntry entry)
        {
            await Task.FromResult(0);
        }
    }
}
