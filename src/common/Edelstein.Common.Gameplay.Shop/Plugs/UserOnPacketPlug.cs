using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Shop;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<IShopStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<IShopStageUser> handler) : base(handler)
    {
    }
}
