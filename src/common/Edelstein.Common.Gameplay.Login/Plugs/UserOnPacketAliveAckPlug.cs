using Edelstein.Common.Gameplay.Handlers;
using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketAliveAckPlug : AbstractAliveAckHandler<ILoginStageUser>
{
    public UserOnPacketAliveAckPlug(IPipeline<UserOnPacketAliveAck<ILoginStageUser>> pipeline) : base(pipeline)
    {
    }
}
