using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Login;

namespace Edelstein.Plugin.Rue;

public class TestPlugin : ILoginPlugin
{
    public string ID => "Rue";
    
    public Task OnStart(IPluginHost<LoginContext> host, LoginContext ctx)
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
