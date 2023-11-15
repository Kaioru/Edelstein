using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Plugin;

namespace Edelstein.Plugin.Rue.Commands.Admin;

public class PluginCommand : AbstractCommand
{
    private readonly IPluginManager<GameContext> _pluginManager;
    
    public PluginCommand(IPluginManager<GameContext> pluginManager) 
        => _pluginManager = pluginManager;
    
    public override string Name => "Plugins";

    public override string Description => "Lists all loaded plugins";
    
    public override async Task Execute(IFieldUser user, string[] args)
    {
        var id = 0;
        var plugins = (await _pluginManager.RetrieveAll())
            .ToImmutableDictionary(
                h => id++,
                h => h
            );
        var pluginMenu = plugins
            .ToImmutableDictionary(
                kv => kv.Key,
                kv => kv.Value.Manifest?.Name ?? kv.Value.ID
            );
        var pluginSelect = await user.Prompt(target => target.AskMenu("Here are the currently loaded plugins", pluginMenu), -1);

        if (pluginSelect == -1) return;

        var plugin = plugins[pluginSelect];

        await user.Prompt(target => target.Say(
            plugin.Manifest == null
                ? "Plugin was not loaded with a manifest"
                : $"Name: {plugin.Manifest.Name}\r\n" +
                  $"Description: {plugin.Manifest.Description}\r\n" +
                  $"EntryPoint: {plugin.Manifest.EntryPoint}"
        ), -1);
    }
}
