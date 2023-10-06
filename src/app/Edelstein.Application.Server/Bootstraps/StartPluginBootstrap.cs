using Edelstein.Application.Server.Configs;
using Edelstein.Protocol.Plugin;
using Microsoft.Extensions.Logging;

namespace Edelstein.Application.Server.Bootstraps;

public class StartPluginBootstrap<TContext> : IBootstrap
{
    private readonly ILogger _logger;
    private readonly ProgramConfigStage _config;
    private readonly TContext _context;
    private readonly IPluginManager<TContext> _manager;
    
    public StartPluginBootstrap(
        ILogger<StartPluginBootstrap<TContext>> logger, 
        ProgramConfigStage config, 
        TContext context, 
        IPluginManager<TContext> manager
    )
    {
        _logger = logger;
        _config = config;
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
            _logger.LogInformation("Initialised plugin {ID} for {Stage} ({Context})", host.ID, _config.ID, typeof(TContext).Name);
        }

        foreach (var host in hosts)
        {
            await host.Plugin.OnStart(host, _context);
            _logger.LogInformation("Started plugin {ID} for {Stage} ({Context})", host.ID, _config.ID, typeof(TContext).Name);
        }
    }

    public async Task Stop()
    {
        var hosts = await _manager.RetrieveAll();

        foreach (var host in hosts)
        {
            await host.Plugin.OnStop();
            _logger.LogInformation("Stopped plugin {ID} for {Stage} ({Context})", host.ID, _config.ID, typeof(TContext).Name);
        }
    }
}
