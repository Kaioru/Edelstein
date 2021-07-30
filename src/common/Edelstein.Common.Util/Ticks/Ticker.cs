using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Edelstein.Common.Util.Ticks
{
    public class Ticker : ITicker
    {
        private readonly CancellationTokenSource _source;
        private readonly Stopwatch _stopwatch;
        private readonly ITickerBehavior _behavior;
        private readonly int _refreshRate;
        private readonly ILogger<ITicker> _logger;

        public Ticker(
            ITickerBehavior behavior,
            int refreshRate = 4,
            ILogger<ITicker> logger = null
        )
        {
            _source = new CancellationTokenSource();
            _stopwatch = new Stopwatch();
            _behavior = behavior;
            _refreshRate = refreshRate;
            _logger = logger ?? new NullLogger<ITicker>();

            _stopwatch.Start();
        }

        public Task Start()
            => Task.Run(async () =>
            {
                var limit = 1 / (float)_refreshRate * 1000;
                var previous = _stopwatch.ElapsedMilliseconds;

                while (!_source.IsCancellationRequested)
                {
                    var start = _stopwatch.ElapsedMilliseconds;
                    var delta = start - previous;

                    previous = start;

                    await Tick();

                    var end = _stopwatch.ElapsedMilliseconds;
                    var duration = end - start;

                    if (duration > limit)
                    {
                        var over = duration - limit;
                        var missed = over / limit;

                        _logger.LogWarning($"Ticker running {over}ms behind, skipping {missed} ticks");
                    }
                    else
                        await Task.Delay(
                            TimeSpan.FromMilliseconds(limit - (duration / limit)),
                            _source.Token
                        );
                }
            }, _source.Token);

        public Task Stop()
        {
            _source.Cancel();
            return Task.CompletedTask;
        }

        public Task Tick() => _behavior.OnTick(DateTime.UtcNow);
    }
}
