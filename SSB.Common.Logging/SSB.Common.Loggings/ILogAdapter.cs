using System.Threading.Tasks;

namespace SSB.Common.Logging
{
    public interface ILogAdapter
    {
        Task Log(LogEntry entry);
    }
}