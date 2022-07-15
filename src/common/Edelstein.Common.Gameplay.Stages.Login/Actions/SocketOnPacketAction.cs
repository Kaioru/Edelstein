using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Actions;
using Edelstein.Protocol.Gameplay.Stages.Login;

namespace Edelstein.Common.Gameplay.Stages.Login.Actions;

public class SocketOnPacketAction : AbstractSocketOnPacketAction<ILoginStageUser>
{
    public SocketOnPacketAction(IPacketHandlerManager<ILoginStageUser> handler) : base(handler)
    {
    }
}
