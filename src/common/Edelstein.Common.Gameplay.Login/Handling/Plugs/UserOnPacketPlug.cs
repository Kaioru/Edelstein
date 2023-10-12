using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Common.Gameplay.Login.Handling.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<ILoginStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<ILoginStageUser> handler) : base(handler)
    {
    }
}
