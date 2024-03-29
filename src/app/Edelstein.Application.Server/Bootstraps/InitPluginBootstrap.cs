﻿using Edelstein.Application.Server.Configs;
using Edelstein.Protocol.Plugin;

namespace Edelstein.Application.Server.Bootstraps;

public class InitPluginBootstrap<TContext> : IBootstrap
{
    private readonly IPluginManager<TContext> _manager;
    private readonly ProgramConfig _config;
    
    public InitPluginBootstrap(IPluginManager<TContext> manager, ProgramConfig config)
    {
        _manager = manager;
        _config = config;
    }

    public int Priority => BootstrapPriority.Init;

    public Task Start() 
        => Task.WhenAll(_config.Plugins.Select(p => 
            File.GetAttributes(Path.GetFullPath(p)).HasFlag(FileAttributes.Directory) 
                ? _manager.LoadFromDirectory(Path.GetFullPath(p)) 
                : _manager.LoadFromFile(Path.GetFullPath(p))));
    
    public Task Stop() => Task.CompletedTask;
}
