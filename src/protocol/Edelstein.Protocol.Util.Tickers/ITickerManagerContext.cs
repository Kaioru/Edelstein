namespace Edelstein.Protocol.Util.Tickers;

public interface ITickerManagerContext : ITickable
{
    DateTime TickPrev { get; set; }
    DateTime TickNext { get; set; }

    bool IsRequestedCancellation { get; }

    void Cancel();
}
