using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Util.Ticks
{
    public class TickerManagerEntry : ITickerManagerEntry
    {
        private ITickerBehavior _behavior;
        private TimeSpan _frequency;
        private bool _executeOnce;

        public DateTime LastTick { get; set; }
        public DateTime NextTick { get; set; }

        public bool IsCancelled { get; set; }

        public Task Cancel() { IsCancelled = true; return Task.CompletedTask; }

        public TickerManagerEntry(ITickerBehavior behavior, TimeSpan? frequency = null)
        {
            _behavior = behavior;
            _frequency = frequency ?? TimeSpan.Zero;
            _executeOnce = frequency == null;
        }

        public async Task OnTick(DateTime now)
        {
            await _behavior.OnTick(now);

            if (_executeOnce)
                await Cancel();

            LastTick = now;
            NextTick = now + _frequency;
        }
    }
}
