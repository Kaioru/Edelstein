using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Plugs;
using Edelstein.Protocol.Gameplay.Trade;

namespace Edelstein.Common.Gameplay.Trade.Handling.Plugs;

public class UserOnPacketPlug : AbstractUserOnPacketPlug<ITradeStageUser>
{
    public UserOnPacketPlug(IPacketHandlerManager<ITradeStageUser> handler) : base(handler)
    {
    }
}
