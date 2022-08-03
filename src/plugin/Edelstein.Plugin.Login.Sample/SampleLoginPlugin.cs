using Edelstein.Plugin.Login.Sample.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Login;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Login.Sample;

public class SampleLoginPlugin : ILoginPlugin
{
    public string ID => "SampleLoginPlugin";

    public async Task OnStart(IPluginHost<ILoginContext> host, ILoginContext ctx)
    {
        var dependency = await host.Plugins.Retrieve("SampleLoginPlugin2");

        if (dependency != null)
        {
            var method = dependency.GetType().GetMethod("Call");
            method?.Invoke(dependency, null);
        }

        ctx.Pipelines.CheckPassword.Add(PipelinePriority.Highest, new SampleCheckPasswordPlug(host.Logger));
        ctx.Pipelines.SocketOnMigrateOut.Add(PipelinePriority.Highest, new SampleSocketOnMigrateOutPlug(host.Logger));
        host.Logger.LogInformation("Sample login plugin registered custom pipeline plugs");
    }

    public Task OnStop() => Task.CompletedTask;
}
