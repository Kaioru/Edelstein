using System;

namespace Edelstein.Protocol.Util.Ticks
{
    public interface ITickerManager : ITickerBehavior
    {
        ITickerManagerEntry Schedule(ITickerBehavior behavior);
        ITickerManagerEntry Schedule(ITickerBehavior behavior, TimeSpan frequency);
        ITickerManagerEntry Schedule(ITickerBehavior behavior, TimeSpan frequency, TimeSpan delay);

        ITickerManagerEntry Execute(ITickerBehavior behavior);
        ITickerManagerEntry Execute(ITickerBehavior behavior, TimeSpan delay);
        ITickerManagerEntry Execute(ITickerBehavior behavior, DateTime date);
    }
}
