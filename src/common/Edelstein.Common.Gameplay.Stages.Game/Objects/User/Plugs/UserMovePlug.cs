using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Plugs;

public class UserMovePlug : IPipelinePlug<IUserMove>
{
    public Task Handle(IPipelineContext ctx, IUserMove message) =>
        message.User.Move(message.Path);
}
