using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop;

namespace Edelstein.Common.Gameplay.Shop;

public static class ShopStageUserExtensions
{
    public static Task DispatchUpdateCash(this IShopStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.CashShopQueryCashResult);

        p.WriteInt(user.Account?.NexonCash ?? -1);
        p.WriteInt(user.Account?.MaplePoint ?? -1);
        p.WriteInt(user.Account?.PrepaidNXCash ?? -1);
        return user.Dispatch(p.Build());
    }
}
