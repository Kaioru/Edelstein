using System.Collections.Frozen;
using System.Collections.Immutable;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Utilities.Tickers;

public class TickerManager : ITickerManager, ITickable
{
    private readonly ICollection<ITickerManagerContext> _tickables;
    private readonly ITicker _ticker;

    public TickerManager(ILogger<TickerManager> logger, int refreshRate = 4)
    {
        RefreshRate = refreshRate;
        _ticker = new Ticker(logger, this, RefreshRate);
        _tickables = new List<ITickerManagerContext>();
    }

    public async Task OnTick(DateTime now)
    {
        var tickables = _tickables.ToFrozenSet();

        await Task.WhenAll(tickables.Select(b =>
            Task.Run(async () =>
            {
                if (!b.IsRequestedCancellation && now >= b.TickNext)
                    await b.OnTick(now);
            })
        ));

        foreach (var b in tickables.Where(b => b.IsRequestedCancellation))
            _tickables.Remove(b);
    }

    public int RefreshRate { get; }

    public Task Start()
    {
        _ticker.Start();
        return Task.CompletedTask;
    }

    public Task Stop()
    {
        _ticker.Stop();
        return Task.CompletedTask;
    }

    public ITickerManagerContext Schedule(ITickable tickable)
        => Schedule(tickable, TimeSpan.Zero);

    public ITickerManagerContext Schedule(ITickable tickable, TimeSpan frequency)
        => Schedule(tickable, frequency, frequency);

    public ITickerManagerContext Schedule(ITickable tickable, TimeSpan frequency, TimeSpan delay)
    {
        var ctx = new TickerManagerContext(tickable, frequency) { TickNext = DateTime.UtcNow + delay };
        _tickables.Add(ctx);
        return ctx;
    }
}
