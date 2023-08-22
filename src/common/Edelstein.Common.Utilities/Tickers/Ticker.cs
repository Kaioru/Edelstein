using System.Diagnostics;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Utilities.Tickers;

public class Ticker : ITicker
{
    private readonly CancellationTokenSource _cts;
    private readonly ILogger _logger;
    private readonly ITickable _tickable;

    public Ticker(ILogger logger, ITickable tickable, int refreshRate)
    {
        _logger = logger;
        _tickable = tickable;
        _cts = new CancellationTokenSource();
        RefreshRate = refreshRate;
    }

    public int RefreshRate { get; }

    public Task Start()
        => Task.Run(async () =>
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var limit = 1 / (float)RefreshRate * 1000;
            var previous = stopwatch.ElapsedMilliseconds;

            while (!_cts.IsCancellationRequested)
            {
                var start = stopwatch.ElapsedMilliseconds;
                var delta = start - previous;

                previous = start;

                await _tickable.OnTick(DateTime.UtcNow);

                var end = stopwatch.ElapsedMilliseconds;
                var duration = end - start;

                if (duration > limit)
                {
                    var over = duration - limit;
                    var missed = over / limit;

                    _logger.LogWarning("Ticker running {Over:F2}ms behind, skipping {Missed:F2} ticks", over, missed);
                }
                else
                {
                    await Task.Delay(
                        TimeSpan.FromMilliseconds(limit - duration / limit),
                        _cts.Token
                    );
                }
            }
        }, _cts.Token);

    public Task Stop()
    {
        _cts.Cancel();
        return Task.CompletedTask;
    }
}
