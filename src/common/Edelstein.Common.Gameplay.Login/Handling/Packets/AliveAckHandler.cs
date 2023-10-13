using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class AliveAckHandler : AbstractAliveAckHandler<ILoginStageUser>
{
    public AliveAckHandler(IPipeline<UserOnPacketAliveAck<ILoginStageUser>> pipeline) : base(pipeline)
    {
    }
}
