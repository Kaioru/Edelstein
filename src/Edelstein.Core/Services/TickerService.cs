using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Ticks;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Services
{
    public class TickerService : IHostedService
    {
        private readonly ITicker _ticker;

        public TickerService(ITicker ticker)
            => _ticker = ticker;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ticker.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _ticker.Stop();
            return Task.CompletedTask;
        }
    }
}