using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Application.Server.Bootstraps;

public class InitTickerBootstrap : IBootstrap
{
    private readonly ITickerManager _ticker;

    public InitTickerBootstrap(ITickerManager ticker) => _ticker = ticker;
    public int Priority => BootstrapPriority.Init;

    public Task Start() => _ticker.Start();
    public Task Stop() => _ticker.Stop();
}
