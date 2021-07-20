using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITickerManager : ITicker
    {
        Task<ITickerManagerEntry> Schedule(ITickerBehavior behavior, TimeSpan time);
        Task<ITickerManagerEntry> Execute(ITickerBehavior behavior);
    }
}
