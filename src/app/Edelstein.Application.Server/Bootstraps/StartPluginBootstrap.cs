using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class StartPluginBootstrap<TContext> : IBootstrap
{
    private readonly ILogger _logger;
    private readonly TContext _context;
    private readonly IPluginManager<TContext> _manager;

    public StartPluginBootstrap(
        ILogger<StartPluginBootstrap<TContext>> logger,
        TContext context,
        IPluginManager<TContext> manager
    )
    {
        _logger = logger;
        _context = context;
        _manager = manager;
    }

    public int Priority => BootstrapPriority.Start;

    public async Task Start()
    {
        var hosts = await _manager.RetrieveAll();
        
        foreach (var host in hosts)
        {
            await host.Plugin.OnInit(host, _context);
            _logger.LogInformation("{Context} plugin {ID} initialised", typeof(TContext).Name, host.ID);
        }

        foreach (var host in hosts)
        {
            await host.Plugin.OnStart(host, _context);
            _logger.LogInformation("{Context} plugin {ID} started", typeof(TContext).Name, host.ID);
        }
    }

    public async Task Stop()
    {
        var hosts = await _manager.RetrieveAll();

        foreach (var host in hosts)
        {
            await host.Plugin.OnStop();
            _logger.LogInformation("{Context} plugin {ID} stopped", typeof(TContext).Name, host.ID);
        }
    }
}
