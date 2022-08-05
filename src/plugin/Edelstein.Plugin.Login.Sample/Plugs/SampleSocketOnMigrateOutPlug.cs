using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Login.Sample.Plugs;

public class SampleSocketOnMigrateOutPlug : IPipelinePlug<ISocketOnMigrateOut<ILoginStageUser>>
{
    private readonly ILogger _logger;

    public SampleSocketOnMigrateOutPlug(ILogger logger) => _logger = logger;

    public Task Handle(IPipelineContext ctx, ISocketOnMigrateOut<ILoginStageUser> message)
    {
        _logger.LogInformation(
            "{Name} is migrating to {Server}",
            message.User.Character!.Name, message.ServerID
        );
        return Task.CompletedTask;
    }
}
