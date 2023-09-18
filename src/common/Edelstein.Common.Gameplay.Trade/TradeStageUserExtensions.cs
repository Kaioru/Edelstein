using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Trade;

namespace Edelstein.Common.Gameplay.Trade;

public static class TradeStageUserExtensions
{
    public static Task DispatchUpdateCash(this ITradeStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.ITCQueryCashResult);

        p.WriteInt(user.Account?.NexonCash ?? -1);
        p.WriteInt(user.Account?.MaplePoint ?? -1);
        return user.Dispatch(p.Build());
    }
}
