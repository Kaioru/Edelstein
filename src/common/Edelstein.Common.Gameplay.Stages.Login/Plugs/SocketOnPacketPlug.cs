using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Actions;
using Edelstein.Protocol.Gameplay.Stages.Login;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class SocketOnPacketPlug : AbstractSocketOnPacketPlug<ILoginStageUser>
{
    public SocketOnPacketPlug(IPacketHandlerManager<ILoginStageUser> handler) : base(handler)
    {
    }
}
