using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Login;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Login.Sample2;

public class SampleLoginPlugin2 : ILoginPlugin
{
    private IPluginHost<ILoginContext>? Host { get; set; }
    public string ID => "SampleLoginPlugin2";

    public Task OnStart(IPluginHost<ILoginContext> host, ILoginContext ctx)
    {
        Host = host;
        return Task.CompletedTask;
    }

    public Task OnStop() => Task.CompletedTask;

    public void Call() => Host?.Logger.LogInformation("{ID} called from external plugin!", ID);
}
