using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<ILoginStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<ILoginStageUser> handler) : base(handler)
    {
    }
}
