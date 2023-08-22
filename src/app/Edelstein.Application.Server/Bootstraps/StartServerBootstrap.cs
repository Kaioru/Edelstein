using Edelstein.Application.Server.Configs;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Utilities.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class StartServerBootstrap : IBootstrap, ITickable
{
    private readonly ILogger _logger;
    private readonly ITickerManager _ticker;
    private readonly ITransportAcceptor _acceptor;
    private readonly ProgramConfigStage _config;
    
    public int Priority => BootstrapPriority.Start;
    private ITickerManagerContext? TickerContext { get; set; }
    private ITransportContext? TransportContext { get; set; }

    public StartServerBootstrap(
        ILogger<StartServerBootstrap> logger,
        ITickerManager ticker, 
        ITransportAcceptor acceptor, 
        ProgramConfigStage config)
    {
        _logger = logger;
        _ticker = ticker;
        _acceptor = acceptor;
        _config = config;
    }

    public async Task Start() => TickerContext = _ticker.Schedule(this, TimeSpan.FromMinutes(10), TimeSpan.Zero);

    public async Task Stop()
    {
        TickerContext?.Cancel();
        if (TransportContext != null)
            await TransportContext.Close();
    }

    public async Task OnTick(DateTime now)
    {
        if (TransportContext == null || TransportContext?.State == TransportState.Closed)
        {
            TransportContext = await _acceptor.Accept(_config.Host, _config.Port);
            _logger.LogInformation(
                "{ID} socket acceptor for v{Version}.{Patch} (Locale {Locale}) bound at {Host}:{Port}",
                _config.ID,
                _config.Version, _config.Patch, _config.Locale,
                _config.Host, _config.Port
            );
        }
    }
}
