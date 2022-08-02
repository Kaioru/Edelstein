using System.Diagnostics;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Common.Util.Templates;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Util.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server.Bootstraps;

public class ServerStartBootstrap : IBootstrap, ITickable
{
    private readonly ITransportAcceptor _acceptor;
    private readonly TimeSpan _aliveFrequency;
    private readonly TimeSpan _aliveSchedule;
    private readonly AbstractProgramConfigStage _config;
    private readonly ICollection<ITemplateLoader> _loaders;
    private readonly ILogger _logger;
    private readonly ITickerManager _tickerManager;

    public ServerStartBootstrap(
        ILogger<ServerStartBootstrap> logger,
        ITickerManager tickerManager,
        ITransportAcceptor acceptor,
        AbstractProgramConfigStage config,
        ICollection<ITemplateLoader> loaders
    )
    {
        _logger = logger;
        _tickerManager = tickerManager;
        _acceptor = acceptor;
        _config = config;
        _loaders = loaders;
        _aliveFrequency = _acceptor.Timeout.Divide(2);
        _aliveSchedule = _acceptor.Timeout.Divide(4);
        AliveLast = DateTime.UtcNow;
    }

    private DateTime AliveLast { get; set; }

    private ITickerManagerContext? Context { get; set; }

    public async Task Start()
    {
        var stopwatch = new Stopwatch();

        foreach (var loader in _loaders)
        {
            stopwatch.Start();

            var count = await loader.Load();

            _logger.LogInformation(
                "{Loader} initialized {Count} templates in {Elapsed}",
                loader.GetType().Name, count, stopwatch.Elapsed
            );
        }

        await _acceptor.Accept(_config.Host, _config.Port);

        Context = _tickerManager.Schedule(this);

        _logger.LogInformation(
            "{ID} socket acceptor bound at {Host}:{Port}",
            _config.ID, _config.Host, _config.Port
        );
    }

    public async Task Stop()
    {
        await _acceptor.Close();
        Context?.Cancel();
        _logger.LogInformation(
            "{ID} socket acceptor closed",
            _config.ID
        );
    }

    public async Task OnTick(DateTime now)
    {
        if (now - AliveLast > _aliveSchedule)
        {
            AliveLast = now;

            foreach (var socket in _acceptor.Sockets.Values)
                if (now - socket.LastAliveSent > _aliveFrequency)
                {
                    socket.LastAliveSent = now;
                    await socket.Dispatch(new PacketWriter(PacketSendOperations.AliveReq));
                }
        }
    }
}
