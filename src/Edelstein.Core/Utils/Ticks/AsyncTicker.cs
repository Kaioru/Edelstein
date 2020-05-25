using System;
using System.Threading;
using System.Threading.Tasks;

namespace Edelstein.Core.Utils.Ticks
{
    public class AsyncTicker : ITicker
    {
        private readonly CancellationTokenSource _cts;
        private readonly ITickBehavior _behavior;

        public AsyncTicker(ITickBehavior behavior)
        {
            _cts = new CancellationTokenSource();
            _behavior = behavior;
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                    await _behavior.TryTick(DateTime.UtcNow);
            }, _cts.Token);
        }

        public void Stop()
            => _cts.Cancel();

        public Task ForceTick()
            => _behavior.TryTick(DateTime.UtcNow);
    }
}