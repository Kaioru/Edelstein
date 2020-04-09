using System;
using System.Threading.Tasks;
using System.Timers;

namespace Edelstein.Core.Utils.Ticks
{
    public class TimerTicker : ITicker
    {
        private readonly ITickBehavior _behavior;
        private readonly Timer _timer;

        public TimerTicker(TimeSpan time, ITickBehavior behavior)
        {
            _behavior = behavior;
            _timer = new Timer
            {
                Interval = time.TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += async (sender, args) => await _behavior.TryTick(DateTime.UtcNow);
        }

        public void Start()
            => _timer.Start();

        public void Stop()
            => _timer.Stop();

        public Task ForceTick()
            => _behavior.TryTick(DateTime.UtcNow);
    }
}