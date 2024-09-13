using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Game;

namespace Edelstein.Plugin.Rue;

public class RuePluginGame : IGamePlugin
{
    public string ID => "Rue";
    
    public Task OnStart(IPluginHost<GameContext> host, GameContext ctx)
    {
        Console.WriteLine("Started");
        return Task.CompletedTask;
    }
    
    public Task OnStop()
    {
        Console.WriteLine("Stopped");
        return Task.CompletedTask;
    }
}
