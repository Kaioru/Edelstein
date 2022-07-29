using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Util.Tickers;

public class TickerManagerContext : ITickerManagerContext
{
    private readonly TimeSpan _frequency;
    private readonly ITickable _tickable;

    public TickerManagerContext(ITickable tickable, TimeSpan frequency)
    {
        _tickable = tickable;
        _frequency = frequency;
    }

    public DateTime TickPrev { get; set; }
    public DateTime TickNext { get; set; }

    public bool IsRequestedCancellation { get; private set; }

    public void Cancel() => IsRequestedCancellation = true;

    public async Task OnTick(DateTime now)
    {
        await _tickable.OnTick(now);

        TickPrev = now;
        TickNext = now + _frequency;
    }
}
