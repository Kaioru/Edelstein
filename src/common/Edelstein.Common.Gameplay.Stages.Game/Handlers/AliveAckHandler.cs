using Edelstein.Common.Gameplay.Stages.Handlers;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Handlers;

public class AliveAckHandler : AbstractAliveAckHandler<IGameStageUser>
{
    public AliveAckHandler(IPipeline<ISocketOnAliveAck<IGameStageUser>> pipeline) : base(pipeline)
    {
    }
}
