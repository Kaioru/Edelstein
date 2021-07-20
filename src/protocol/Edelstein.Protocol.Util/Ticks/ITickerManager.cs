using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITickerManager : ITicker
    {
        Task<ITickerManagerEntry> Schedule(ITickerBehavior behavior, TimeSpan frequency);
        Task<ITickerManagerEntry> Schedule(ITickerBehavior behavior, TimeSpan frequency, TimeSpan delay);
        Task<ITickerManagerEntry> Execute(ITickerBehavior behavior);
    }
}
