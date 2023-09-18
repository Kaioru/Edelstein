using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Plugs;
using Edelstein.Protocol.Gameplay.Trade;

namespace Edelstein.Common.Gameplay.Trade.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<ITradeStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<ITradeStageUser> handler) : base(handler)
    {
    }
}
