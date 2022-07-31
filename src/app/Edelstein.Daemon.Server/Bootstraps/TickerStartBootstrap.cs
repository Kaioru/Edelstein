using Edelstein.Protocol.Util.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server.Bootstraps;

public class TickerStartBootstrap : IBootstrap
{
    private readonly ILogger _logger;
    private readonly ITickerManager _ticker;

    public TickerStartBootstrap(ILogger<TickerStartBootstrap> logger, ITickerManager ticker)
    {
        _logger = logger;
        _ticker = ticker;
    }

    public async Task Start()
    {
        await _ticker.Start();
        _logger.LogDebug("Ticker started with refresh rate of {RefreshRate} Hz", _ticker.RefreshRate);
    }

    public Task Stop() => _ticker.Stop();
}
