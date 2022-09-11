using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class WorldSelectResetPlug : IPipelinePlug<IWorldSelectReset>
{
    public Task Handle(IPipelineContext ctx, IWorldSelectReset message)
    {
        message.User.State = LoginState.SelectWorld;
        message.User.AccountWorld = null;
        message.User.SelectedWorldID = null;
        message.User.SelectedChannelID = null;

        return Task.CompletedTask;
    }
}
