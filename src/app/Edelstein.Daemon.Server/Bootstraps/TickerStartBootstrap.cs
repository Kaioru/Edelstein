using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Daemon.Server.Bootstraps;

public class TickerStartBootstrap : IBootstrap
{
    private readonly ITickerManager _ticker;

    public TickerStartBootstrap(ITickerManager ticker) => _ticker = ticker;

    public Task Start() => _ticker.Start();
    public Task Stop() => _ticker.Stop();
}
