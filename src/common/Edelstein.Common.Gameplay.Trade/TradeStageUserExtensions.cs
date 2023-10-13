using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Trade;

namespace Edelstein.Common.Gameplay.Trade;

public static class TradeStageUserExtensions
{
    public static Task DispatchUpdateCash(this ITradeStageUser user)
    {
        using var packet = new PacketWriter(PacketSendOperations.ITCQueryCashResult);

        packet.WriteInt(user.Account?.NexonCash ?? -1);
        packet.WriteInt(user.Account?.MaplePoint ?? -1);
        return user.Dispatch(packet.Build());
    }
}
