using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Utilities.Tickers;

public class TickerManagerContext : ITickerManagerContext
{
    private readonly TimeSpan _frequency;
    private readonly ITickable _tickable;

    public DateTime TickPrev { get; set; }
    public DateTime TickNext { get; set; }

    public bool IsRequestedCancellation { get; private set; }

    public TickerManagerContext(ITickable tickable, TimeSpan frequency)
    {
        _tickable = tickable;
        _frequency = frequency;
    }

    public void Cancel() => IsRequestedCancellation = true;

    public async Task OnTick(DateTime now)
    {
        if (!IsRequestedCancellation)
        {
            TickPrev = now;
            TickNext = now + _frequency;

            await _tickable.OnTick(now);
        }
    }
}
