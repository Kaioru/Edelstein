using Edelstein.Common.Gameplay.Stages.Handlers;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class AliveAckHandler : AbstractAliveAckHandler<ILoginStageUser>
{
    public AliveAckHandler(IPipeline<ISocketOnAliveAck<ILoginStageUser>> pipeline) : base(pipeline)
    {
    }
}
