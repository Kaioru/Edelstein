using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Edelstein.Common.Util.Ticks
{
    public class TickerManager : ITickerManager
    {
        private readonly ITicker _ticker;
        private readonly ICollection<ITickerManagerEntry> _entries;
        private readonly ILogger _logger;

        public TickerManager(int refreshRate = 2, ILogger<ITicker> logger = null)
        {
            _logger = logger ?? new NullLogger<ITicker>();
            _ticker = new Ticker(this, refreshRate, logger);
            _entries = new List<ITickerManagerEntry>();

            _ticker.Start();
        }

        public async Task OnTick(DateTime now)
        {
            var behaviors = _entries.ToImmutableList();

            await Task.WhenAll(behaviors.Select(b =>
                Task.Run(async () =>
                {
                    if (!b.IsCancelled && now >= b.NextTick)
                        await b.OnTick(now);
                })
            ));

            foreach (var b in behaviors.Where(b => b.IsCancelled))
                _entries.Remove(b);
        }

        public ITickerManagerEntry Schedule(ITickerBehavior behavior)
            => Schedule(behavior, TimeSpan.Zero);

        public ITickerManagerEntry Schedule(ITickerBehavior behavior, TimeSpan frequency)
            => Schedule(behavior, frequency, TimeSpan.Zero);

        public ITickerManagerEntry Schedule(ITickerBehavior behavior, TimeSpan frequency, TimeSpan delay)
        {
            var entry = new TickerManagerEntry(behavior, frequency) { NextTick = DateTime.UtcNow + delay };

            _entries.Add(entry);
            return entry;
        }

        public ITickerManagerEntry Execute(ITickerBehavior behavior)
            => Execute(behavior, TimeSpan.Zero);

        public ITickerManagerEntry Execute(ITickerBehavior behavior, TimeSpan delay)
            => Execute(behavior, DateTime.UtcNow + delay);

        public ITickerManagerEntry Execute(ITickerBehavior behavior, DateTime date)
        {
            var entry = new TickerManagerEntry(behavior) { NextTick = date };

            _entries.Add(entry);
            return entry;
        }
    }
}
