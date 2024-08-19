using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Application.Server.Services;

public class TickerHostService(
    ITicker ticker
) : IHostedService
{

    public Task StartAsync(CancellationToken cancellationToken) 
        => Task.CompletedTask;
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        ticker.Dispose();
        return Task.CompletedTask;
    }
}
