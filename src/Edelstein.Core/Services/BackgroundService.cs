using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Services
{
    public class BackgroundService<T> : IHostedService
        where T : class, IHostedService
    {
        private readonly T _service;

        public BackgroundService(T service)
            => _service = service;

        public Task StartAsync(CancellationToken cancellationToken)
            => _service.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => _service.StopAsync(cancellationToken);
    }
}