using Edelstein.Protocol.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Plugs;

public class UserMovePlug : IPipelinePlug<IUserMove>
{
    public Task Handle(IPipelineContext ctx, IUserMove message) =>
        message.User.Move(message.Path);
}
