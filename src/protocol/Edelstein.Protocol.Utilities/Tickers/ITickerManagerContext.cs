namespace Edelstein.Protocol.Utilities.Tickers;

public interface ITickerManagerContext : ITickable
{
    DateTime TickPrev { get; set; }
    DateTime TickNext { get; set; }

    bool IsRequestedCancellation { get; }

    void Cancel();
}
