namespace Edelstein.Protocol.Utilities.Tickers;

public interface ITickerManager : ITicker
{
    ITickerManagerContext Schedule(ITickable tickable);
    ITickerManagerContext Schedule(ITickable tickable, TimeSpan frequency);
    ITickerManagerContext Schedule(ITickable tickable, TimeSpan frequency, TimeSpan delay);
}
