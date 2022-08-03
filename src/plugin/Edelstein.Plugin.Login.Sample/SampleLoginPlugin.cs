using Edelstein.Plugin.Login.Sample.Plugs;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Plugin.Login;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Login.Sample;

public class SampleLoginPlugin : ILoginPlugin
{
    public Task OnStart(ILogger logger, ILoginContext ctx)
    {
        ctx.Pipelines.CheckPassword.Add(PipelinePriority.Highest, new SampleCheckPasswordPlug(logger));
        logger.LogInformation("Sample login plugin registered custom pipeline plugs");
        return Task.CompletedTask;
    }

    public Task OnStop() => Task.CompletedTask;
}
