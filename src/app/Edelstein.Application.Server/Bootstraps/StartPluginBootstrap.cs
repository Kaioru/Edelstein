using System.Collections.Immutable;
using Edelstein.Common.Plugin;
using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class StartPluginBootstrap<TContext> : IBootstrap
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly TContext _context;
    private readonly IPluginManager<TContext> _manager;

    public StartPluginBootstrap(
        ILogger<StartPluginBootstrap<TContext>> logger, 
        ILoggerFactory loggerFactory, 
        TContext context, 
        IPluginManager<TContext> manager
    )
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _context = context;
        _manager = manager;
    }
    public int Priority => BootstrapPriority.Start;

    public async Task Start()
    {
        var plugins = await _manager.RetrieveAll();
        var hosted = plugins
            .Select(p => 
                Tuple.Create(p, new PluginHost(_loggerFactory.CreateLogger(p.GetType()))))
            .ToImmutableList();

        foreach (var host in hosted)
        {
            await host.Item1.OnInit(host.Item2, _context);
            _logger.LogDebug("{Context} plugin {ID} initialised", typeof(TContext).Name, host.Item1.ID);
        }
        
        foreach (var host in hosted)
        {
            await host.Item1.OnStart(host.Item2, _context);
            _logger.LogDebug("{Context} plugin {ID} started", typeof(TContext).Name, host.Item1.ID);
        }
    }

    public async Task Stop()
    {
        var plugins = await _manager.RetrieveAll();

        foreach (var plugin in plugins)
        {
            await plugin.OnStop();
            _logger.LogDebug("{Context} plugin {ID} stopped", typeof(TContext).Name, plugin.ID);
        }
    }
}
