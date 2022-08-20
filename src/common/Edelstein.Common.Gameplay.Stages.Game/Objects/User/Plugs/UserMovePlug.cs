using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserMovePlug : IPipelinePlug<IUserMove>
{
    public Task Handle(IPipelineContext ctx, IUserMove message) =>
        message.User.Move(message.Path);
}
