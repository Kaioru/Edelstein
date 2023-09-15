using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop;
using Edelstein.Protocol.Gameplay.Shop.Commodities;

namespace Edelstein.Common.Gameplay.Shop;

public class ShopStage : AbstractStage<IShopStageUser>, IShopStage
{
    public ShopStage(string id) => ID = id;

    public override string ID { get; }

    public new async Task Enter(IShopStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
        {
            await user.Disconnect();
            return;
        }
        
        var packet = new PacketWriter(PacketSendOperations.SetCashShop);
        
        packet.WriteCharacterData(
            user.Character, 
            DbFlags.Character | 
            DbFlags.Money | 
            DbFlags.ItemSlotEquip | 
            DbFlags.ItemSlotConsume | 
            DbFlags.ItemSlotInstall |
            DbFlags.ItemSlotEtc | 
            DbFlags.ItemSlotCash | 
            DbFlags.InventorySize
        );

        packet.WriteBool(true); // CashShopAuthorized
        packet.WriteString(user.Account.Username);
        
        var notSale = await user.Context.Managers.NotSale.RetrieveAll();
        packet.WriteInt(notSale.Count);
        foreach (var commodity in notSale)
            packet.WriteInt(commodity.ID);

        var modifiedCommodities = await user.Context.Managers.ModifiedCommodity.RetrieveAll();
        packet.WriteShort((short)modifiedCommodities.Count);
        foreach (var modified in modifiedCommodities)
        {
            packet.WriteInt(modified.ID);
            packet.WriteInt((int)modified.Flags);

            if (modified.Flags.HasFlag(CommodityFlags.ItemID))
                packet.WriteInt(modified.ItemID ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.Count))
                packet.WriteShort(modified.Count ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.Priority))
                packet.WriteByte(modified.Priority ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.Price))
                packet.WriteInt(modified.Price ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.Bonus))
                packet.WriteBool(modified.Bonus ?? false);
            if (modified.Flags.HasFlag(CommodityFlags.Period))
                packet.WriteShort(modified.Period ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.ReqPOP))
                packet.WriteShort(modified.ReqPOP ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.ReqLVL))
                packet.WriteShort(modified.ReqLevel ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.MaplePoint))
                packet.WriteInt(modified.MaplePoint ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.Meso))
                packet.WriteInt(modified.Meso ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.ForPremiumUser))
                packet.WriteBool(modified.ForPremiumUser ?? false);
            if (modified.Flags.HasFlag(CommodityFlags.CommodityGender))
                packet.WriteByte(modified.Gender ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.OnSale))
                packet.WriteBool(modified.OnSale ?? false);
            if (modified.Flags.HasFlag(CommodityFlags.Class))
                packet.WriteByte(modified.Class ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.Limit))
                packet.WriteByte(modified.Limit ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.PbCash))
                packet.WriteShort(modified.PbCash ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.PbPoint))
                packet.WriteShort(modified.PbPoint ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.PbGift))
                packet.WriteShort(modified.PbGift ?? 0);
            if (modified.Flags.HasFlag(CommodityFlags.PackageSN))
            {
                packet.WriteByte((byte)(modified.PackageSN?.Count ?? 0));
                foreach (var sn in modified.PackageSN ?? new List<int>())
                    packet.WriteInt(sn);
            }
        }
        
        packet.WriteBool(false); // v49

        packet.WriteBytes(new byte[1080]);
        packet.WriteShort(0); // Stock
        packet.WriteShort(0); // LimitGoods
        packet.WriteShort(0); // ZeroGoods

        packet.WriteBool(false); // EventOn
        packet.WriteInt(200); // HighestCharacterLevelInThisAccount

        await user.Dispatch(packet.Build());
        await user.DispatchUpdateCash();
        await base.Enter(user);
    }
}
