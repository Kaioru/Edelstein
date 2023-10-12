using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Shop.Types;

namespace Edelstein.Common.Gameplay.Shop;

public static class ShopStageUserExtensions
{
    public static bool CheckCash(this IShopStageUser user, ShopCashType type, int amount)
    {
        switch (type)
        {
            case ShopCashType.NexonCash:
                return user.Account?.NexonCash >= amount;
            case ShopCashType.MaplePoint:
                return user.Account?.MaplePoint >= amount;
            case ShopCashType.PrepaidNXCash:
                return user.Account?.PrepaidNXCash >= amount;
            default:
                return false;
        }
    }
    
    public static void IncCash(this IShopStageUser user, ShopCashType type, int amount)
    {
        switch (type)
        {
            case ShopCashType.NexonCash:
                if (user.Account?.NexonCash != null) 
                    user.Account.NexonCash += amount;
                break;
            case ShopCashType.MaplePoint:
                if (user.Account?.MaplePoint != null) 
                    user.Account.MaplePoint += amount;
                break;
            case ShopCashType.PrepaidNXCash:
                if (user.Account?.PrepaidNXCash != null) 
                    user.Account.PrepaidNXCash += amount;
                break;
        }
    }
    
    public static Task DispatchUpdateCash(this IShopStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.CashShopQueryCashResult);

        p.WriteInt(user.Account?.NexonCash ?? -1);
        p.WriteInt(user.Account?.MaplePoint ?? -1);
        p.WriteInt(user.Account?.PrepaidNXCash ?? -1);
        return user.Dispatch(p.Build());
    }

    public static Task DispatchUpdateLocker(this IShopStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        p.WriteByte((byte)ShopResultOperations.LoadLocker_Done);
        p.WriteShort((short)(user.AccountWorld?.Locker.Items.Count ?? 0));
        foreach (var slot in user.AccountWorld?.Locker.Items ?? ImmutableList<IItemLockerSlot>.Empty)
            p.WriteItemLockerData(slot);
        p.WriteShort(user.AccountWorld?.Trunk.SlotMax ?? 0);
        p.WriteShort((short)(user.AccountWorld?.CharacterSlotMax ?? 3));
        p.WriteShort((short)((user.AccountWorld?.CharacterSlotMax ?? 0) - 3));
        p.WriteShort(1);
        return user.Dispatch(p.Build());
    }
    
    public static Task DispatchUpdateWish(this IShopStageUser user)
    {
        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        p.WriteByte((byte)ShopResultOperations.LoadWish_Done);
        for (var i = 0; i < 10; i++)
            p.WriteInt(user.Character?.Wishlist.Records.ElementAtOrDefault(i) ?? 0);
        return user.Dispatch(p.Build());
    }
}
