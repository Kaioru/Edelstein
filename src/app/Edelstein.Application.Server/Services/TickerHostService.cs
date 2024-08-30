using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Application.Server.Services;

public class TickerHostService(
    ITicker ticker,
    IEnumerable<ITickerAction> actions
) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var action in actions)
            _ = ticker.Register(action);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        ticker.Dispose();
        return Task.CompletedTask;
    }
}
