using System.Diagnostics;
using Edelstein.Common.Util.Templates;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Protocol.Network.Transports;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server.Bootstraps;

public class ServerStartBootstrap : IBootstrap
{
    private readonly ITransportAcceptor _acceptor;
    private readonly AbstractProgramConfigStage _config;
    private readonly ICollection<ITemplateLoader> _loaders;

    private readonly ILogger _logger;

    public ServerStartBootstrap(
        ILogger<ServerStartBootstrap> logger,
        ITransportAcceptor acceptor,
        AbstractProgramConfigStage config,
        ICollection<ITemplateLoader> loaders
    )
    {
        _logger = logger;
        _acceptor = acceptor;
        _config = config;
        _loaders = loaders;
    }

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

        _logger.LogInformation(
            "{ID} socket acceptor bound at {Host}:{Port}",
            _config.ID, _config.Host, _config.Port
        );
    }

    public async Task Stop()
    {
        await _acceptor.Close();
        _logger.LogInformation(
            "{ID} socket acceptor closed",
            _config.ID
        );
    }
}
