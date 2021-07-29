using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITickerManagerEntry : ITickerBehavior
    {
        DateTime LastTick { get; }
        DateTime NextTick { get; }

        bool IsCancelled { get; }

        Task Cancel();
    }
}
