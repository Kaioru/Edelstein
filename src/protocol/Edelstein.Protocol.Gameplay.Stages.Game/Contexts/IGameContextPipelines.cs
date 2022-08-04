using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextPipelines : IStageContextPipelines<IGameStageUser>
{
    IPipeline<IUserTransferChannelRequest> UserTransferChannelRequest { get; }
    IPipeline<IUserMove> UserMove { get; }
    IPipeline<IUserChat> UserChat { get; }
}
