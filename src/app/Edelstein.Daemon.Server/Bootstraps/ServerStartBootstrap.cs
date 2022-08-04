using System.Collections.Immutable;
using System.Diagnostics;
using Edelstein.Common.Gameplay.Stages.Game.Continents;
using Edelstein.Common.Util.Templates;
using Edelstein.Daemon.Server.Configs;
using Edelstein.Daemon.Server.Tickers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Network.Transports;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Util.Tickers;
using Microsoft.Extensions.Logging;

namespace Edelstein.Daemon.Server.Bootstraps;

public class ServerStartBootstrap<TStage, TStageUser, TContext> : IBootstrap
    where TStage : IStage<TStageUser>
    where TStageUser : IStageUser<TStageUser>
{
    private readonly ITransportAcceptor _acceptor;
    private readonly ProgramConfig _config;
    private readonly TContext _context;
    private readonly ICollection<ITickerManagerContext> _contexts;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IPluginManager<TContext> _pluginManager;
    private readonly AbstractProgramConfigStage _stageConfig;
    private readonly ICollection<ITemplateLoader> _templateLoaders;
    private readonly ITickerManager _ticker;

    public ServerStartBootstrap(
        ILogger<ServerStartBootstrap<TStage, TStageUser, TContext>> logger,
        ILoggerFactory loggerFactory,
        TContext context,
        ProgramConfig config,
        AbstractProgramConfigStage stageConfig,
        ITransportAcceptor acceptor,
        ITickerManager ticker,
        IPluginManager<TContext> pluginManager,
        IEnumerable<ITemplateLoader> templateLoaders
    )
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _context = context;
        _config = config;
        _stageConfig = stageConfig;
        _acceptor = acceptor;
        _ticker = ticker;
        _pluginManager = pluginManager;
        _templateLoaders = templateLoaders.ToImmutableList();
        _contexts = new List<ITickerManagerContext>();
    }

    public async Task Start()
    {
        var stopwatch = new Stopwatch();

        foreach (var loader in _templateLoaders)
        {
            stopwatch.Start();

            var count = await loader.Load();

            _logger.LogInformation(
                "{Loader} initialized {Count} templates in {Elapsed}",
                loader.GetType().Name, count, stopwatch.Elapsed
            );
        }

        _contexts.Add(_ticker.Schedule(new AliveTicker(_acceptor)));

        foreach (var path in _config.Plugins)
        {
            if (!Directory.Exists(path))
            {
                _logger.LogWarning(
                    "Skipping loading plugins from {Path} as it does not exist",
                    Path.GetFullPath(path)
                );
                continue;
            }

            await _pluginManager.LoadFrom(path);
            _logger.LogInformation(
                "Loaded plugin assemblies from {Path}",
                Path.GetFullPath(path)
            );
        }

        if (_context is IGameContext game)
            await Task.WhenAll((await game.Templates.ContiMove.RetrieveAll())
                .Select(t => new ContiMove(_loggerFactory.CreateLogger<ContiMove>(), game.Managers.Field, t))
                .Select(game.Managers.ContiMove.Insert)
            );


        await _pluginManager.Start();
        await _acceptor.Accept(_stageConfig.Host, _stageConfig.Port);

        _logger.LogInformation(
            "{ID} socket acceptor bound at {Host}:{Port}",
            _stageConfig.ID, _stageConfig.Host, _stageConfig.Port
        );
    }

    public async Task Stop()
    {
        foreach (var context in _contexts)
            context.Cancel();

        await _pluginManager.Stop();
        await _acceptor.Close();

        _logger.LogInformation(
            "{ID} socket acceptor closed",
            _stageConfig.ID
        );
    }
}
