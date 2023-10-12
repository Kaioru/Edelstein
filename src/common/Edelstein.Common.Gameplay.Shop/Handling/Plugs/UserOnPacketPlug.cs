using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Shop;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<IShopStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<IShopStageUser> handler) : base(handler)
    {
    }
}
