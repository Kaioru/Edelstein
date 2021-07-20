using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITickerManagerEntry
    {
        ITickerBehavior Behavior { get; }

        DateTime LastTick { get; }
        DateTime NextTick { get; }

        Task Cancel();
    }
}
