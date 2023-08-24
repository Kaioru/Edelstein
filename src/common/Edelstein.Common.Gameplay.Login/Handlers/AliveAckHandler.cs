using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class AliveAckHandler : AbstractAliveAckHandler<ILoginStageUser>
{
    public AliveAckHandler(IPipeline<UserOnPacketAliveAck<ILoginStageUser>?> pipeline) : base(pipeline)
    {
    }
}
