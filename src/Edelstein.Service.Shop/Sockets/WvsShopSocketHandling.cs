using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Inventories;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Server.Best;
using Edelstein.Provider.Templates.Server.CategoryDiscount;
using Edelstein.Provider.Templates.Server.ModifiedCommodity;
using Edelstein.Provider.Templates.Server.NotSale;
using Edelstein.Service.Shop.Logging;
using Edelstein.Service.Shop.Types;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Edelstein.Service.Shop.Sockets
{
    public partial class WvsShopSocket
    {
        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                case RecvPacketOperations.MigrateIn:
                    return OnMigrateIn(packet);
                case RecvPacketOperations.UserTransferFieldRequest:
                    return OnTransferFieldRequest(packet);
                case RecvPacketOperations.CashShopQueryCashRequest:
                    return OnCashShopQueryCashRequest(packet);
                case RecvPacketOperations.CashShopCashItemRequest:
                    return OnCashShopCashItemRequest(packet);
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    return Task.CompletedTask;
            }
        }


        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            using (var db = WvsShop.DataContextFactory.Create())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Locker)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Trunk)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Single(c => c.ID == characterID);

                if (!await WvsShop.TryMigrateFrom(character, WvsShop.Info))
                    await Disconnect();
                Character = character;

                using (var p = new Packet(SendPacketOperations.SetCashShop))
                {
                    character.EncodeData(p);

                    p.Encode<bool>(true); // m_bCashShopAuthorized
                    p.Encode<string>(character.Data.Account.Username); // m_sNexonClubID

                    var templates = WvsShop.TemplateManager;

                    var notSales = templates.GetAll<NotSaleTemplate>().ToList();
                    p.Encode<int>(notSales.Count);
                    notSales.ForEach(n => p.Encode<int>(n.ID));

                    var modifiedCommodities = templates.GetAll<ModifiedCommodityTemplate>().ToList();
                    p.Encode<short>((short) modifiedCommodities.Count);
                    modifiedCommodities.ForEach(m =>
                    {
                        var flag = 0;

                        if (m.ItemID.HasValue) flag |= 0x1;
                        if (m.Count.HasValue) flag |= 0x2;
                        if (m.Priority.HasValue) flag |= 0x10;
                        if (m.Price.HasValue) flag |= 0x4;
                        if (m.Bonus.HasValue) flag |= 0x8;
                        if (m.Period.HasValue) flag |= 0x20;
                        if (m.ReqPOP.HasValue) flag |= 0x20000;
                        if (m.ReqLEV.HasValue) flag |= 0x40000;
                        if (m.MaplePoint.HasValue) flag |= 0x40;
                        if (m.Meso.HasValue) flag |= 0x80;
                        if (m.ForPremiumUser.HasValue) flag |= 0x100;
                        if (m.Gender.HasValue) flag |= 0x200;
                        if (m.OnSale.HasValue) flag |= 0x400;
                        if (m.Class.HasValue) flag |= 0x800;
                        if (m.Limit.HasValue) flag |= 0x1000;
                        if (m.PbCash.HasValue) flag |= 0x2000;
                        if (m.PbPoint.HasValue) flag |= 0x4000;
                        if (m.PbGift.HasValue) flag |= 0x8000;
                        if (m.PackageSN != null) flag |= 0x10000;

                        p.Encode<int>(m.ID);
                        p.Encode<int>(flag);

                        if ((flag & 0x1) != 0) p.Encode<int>(m.ItemID.Value);
                        if ((flag & 0x2) != 0) p.Encode<short>(m.Count.Value);
                        if ((flag & 0x10) != 0) p.Encode<byte>(m.Priority.Value);
                        if ((flag & 0x4) != 0) p.Encode<int>(m.Price.Value);
                        if ((flag & 0x8) != 0) p.Encode<byte>(m.Bonus.Value);
                        if ((flag & 0x20) != 0) p.Encode<short>(m.Period.Value);
                        if ((flag & 0x20000) != 0) p.Encode<short>(m.ReqPOP.Value);
                        if ((flag & 0x40000) != 0) p.Encode<short>(m.ReqLEV.Value);
                        if ((flag & 0x40) != 0) p.Encode<int>(m.MaplePoint.Value);
                        if ((flag & 0x80) != 0) p.Encode<int>(m.Meso.Value);
                        if ((flag & 0x100) != 0) p.Encode<bool>(m.ForPremiumUser.Value);
                        if ((flag & 0x200) != 0) p.Encode<byte>(m.Gender.Value);
                        if ((flag & 0x400) != 0) p.Encode<bool>(m.OnSale.Value);
                        if ((flag & 0x800) != 0) p.Encode<byte>(m.Class.Value);
                        if ((flag & 0x1000) != 0) p.Encode<byte>(m.Limit.Value);
                        if ((flag & 0x2000) != 0) p.Encode<short>(m.PbCash.Value);
                        if ((flag & 0x4000) != 0) p.Encode<short>(m.PbPoint.Value);
                        if ((flag & 0x8000) != 0) p.Encode<short>(m.PbGift.Value);
                        if ((flag & 0x10000) == 0) return;
                        p.Encode<byte>((byte) m.PackageSN.Length);
                        m.PackageSN.ForEach(sn => p.Encode<int>(sn));
                    });

                    var categoryDiscounts = templates.GetAll<CategoryDiscountTemplate>().ToList();
                    p.Encode<byte>((byte) categoryDiscounts.Count);
                    categoryDiscounts.ForEach(d =>
                    {
                        p.Encode<byte>(d.Category);
                        p.Encode<byte>(d.CategorySub);
                        p.Encode<byte>(d.DiscountRate);
                    });

                    const int bestLimit = 90;
                    var best = templates.GetAll<BestTemplate>().ToList();
                    best.Take(bestLimit).ForEach(b =>
                    {
                        p.Encode<int>(b.Category);
                        p.Encode<int>(b.Gender);
                        p.Encode<int>(b.CommoditySN);
                    });
                    if (best.Count < bestLimit)
                    {
                        Enumerable.Repeat(0, bestLimit - best.Count).ForEach(i =>
                        {
                            p.Encode<int>(0);
                            p.Encode<int>(0);
                            p.Encode<int>(0);
                        });
                    }

                    // DecodeStock
                    p.Encode<short>(0);
                    // DecodeLimitGoods
                    p.Encode<short>(0);
                    // DecodeZeroGoods
                    p.Encode<short>(0);

                    p.Encode<bool>(false); // m_bEventOn
                    p.Encode<int>(0); // m_nHighestCharacterLevelInThisAccount
                    await SendPacket(p);
                }

                await SendLockerData();
                await SendCashData();
            }
        }

        private async Task OnTransferFieldRequest(IPacket packet)
        {
            var service = WvsShop.Peers
                .OfType<GameServiceInfo>()
                .Where(g => g.WorldID == Character.Data.WorldID)
                .FirstOrDefault(g => g.Name == Character.Data.Account.PreviousConnectedService);

            if (service != null &&
                !await WvsShop.TryMigrateTo(this, Character, service))
                await Disconnect();
        }

        public async Task SendCashData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopQueryCashResult))
            {
                p.Encode<int>(Character.Data.Account.NexonCash);
                p.Encode<int>(Character.Data.Account.MaplePoint);
                p.Encode<int>(Character.Data.Account.PrepaidNXCash);
                await SendPacket(p);
            }
        }

        public async Task SendLockerData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.LoadLocker_Done);

                var locker = Character.Data.Locker;

                p.Encode<short>((short) locker.Items.Count);
                locker.Items.ForEach(i => i.Encode(p));

                p.Encode<short>(Character.Data.Trunk.SlotMax);
                p.Encode<short>((short) Character.Data.SlotCount);
                p.Encode<short>(0);
                p.Encode<short>((short) Character.Data.Characters.Count);
                await SendPacket(p);
            }
        }

        private Task OnCashShopQueryCashRequest(IPacket packet) => SendCashData();

        private async Task OnCashShopCashItemRequest(IPacket packet)
        {
            var type = (CashItemRequest) packet.Decode<byte>();

            Console.WriteLine(type);
            switch (type)
            {
                case CashItemRequest.WebShopOrderGetList:
                    break;
                case CashItemRequest.LoadLocker:
                    break;
                case CashItemRequest.LoadWish:
                    break;
                case CashItemRequest.Buy:
                    break;
                case CashItemRequest.Gift:
                    break;
                case CashItemRequest.SetWish:
                    break;
                case CashItemRequest.IncSlotCount:
                    break;
                case CashItemRequest.IncTrunkCount:
                    break;
                case CashItemRequest.IncCharSlotCount:
                    break;
                case CashItemRequest.IncBuyCharCount:
                    break;
                case CashItemRequest.EnableEquipSlotExt:
                    break;
                case CashItemRequest.CancelPurchase:
                    break;
                case CashItemRequest.ConfirmPurchase:
                    break;
                case CashItemRequest.Destroy:
                    break;
                case CashItemRequest.MoveLtoS:
                {
                    var sn = packet.Decode<long>();
                    var locker = Character.Data.Locker;
                    var slot = locker.Items.FirstOrDefault(i => i.SN == sn);

                    if (slot == null) return;

                    var context = new ModifyInventoryContext(Character);
                    var template = WvsShop.TemplateManager.Get<ItemTemplate>(slot.ItemID);
                    var item = template.ToItemSlot();

                    item.CashItemSN = slot.SN;
                    item.DateExpire = slot.DateExpire;

                    if (item is ItemSlotBundle b)
                        b.Number = slot.Number;

                    if (!Character.HasSlotFor(item)) return;

                    locker.Items.Remove(slot);
                    context.Add(item);

                    using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
                    {
                        p.Encode<byte>((byte) CashItemResult.MoveLtoS_Done);
                        p.Encode<short>(item.Position);

                        if (item is ItemSlotEquip equip) equip.Encode(p);
                        if (item is ItemSlotBundle bundle) bundle.Encode(p);
                        if (item is ItemSlotPet pet) pet.Encode(p);
                        await SendPacket(p);
                    }

                    break;
                }
                case CashItemRequest.MoveStoL:
                {
                    var id = packet.Decode<long>();
                    var inventories = Character.Inventories;
                    var item = inventories
                        .SelectMany(i => i.Items)
                        .FirstOrDefault(i => i.CashItemSN == id);

                    if (item?.CashItemSN == null) return;

                    var context = new ModifyInventoryContext(Character);
                    var locker = Character.Data.Locker;
                    var slot = new ItemLockerSlot
                    {
                        SN = item.CashItemSN.Value,
                        ItemID = item.TemplateID,
                        DateExpire = item.DateExpire
                    };


                    if (item is ItemSlotBundle b)
                        slot.Number = b.Number;
                    else slot.Number = 1;

                    if (locker.Items.Count >= locker.SlotMax) return;

                    context.Remove(item);
                    locker.Items.Add(slot);

                    using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
                    {
                        p.Encode<byte>((byte) CashItemResult.MoveStoL_Done);
                        slot.Encode(p);
                        await SendPacket(p);
                    }

                    break;
                }
                case CashItemRequest.Expire:
                    break;
                case CashItemRequest.Use:
                    break;
                case CashItemRequest.StatChange:
                    break;
                case CashItemRequest.SkillChange:
                    break;
                case CashItemRequest.SkillReset:
                    break;
                case CashItemRequest.DestroyPetItem:
                    break;
                case CashItemRequest.SetPetName:
                    break;
                case CashItemRequest.SetPetLife:
                    break;
                case CashItemRequest.SetPetSkill:
                    break;
                case CashItemRequest.SetItemName:
                    break;
                case CashItemRequest.SendMemo:
                    break;
                case CashItemRequest.GetMaplePoint:
                    break;
                case CashItemRequest.Rebate:
                    break;
                case CashItemRequest.UseCoupon:
                    break;
                case CashItemRequest.GiftCoupon:
                    break;
                case CashItemRequest.Couple:
                    break;
                case CashItemRequest.BuyPackage:
                    break;
                case CashItemRequest.GiftPackage:
                    break;
                case CashItemRequest.BuyNormal:
                    break;
                case CashItemRequest.ApplyWishListEvent:
                    break;
                case CashItemRequest.MovePetStat:
                    break;
                case CashItemRequest.FriendShip:
                    break;
                case CashItemRequest.ShopScan:
                    break;
                case CashItemRequest.LoadPetExceptionList:
                    break;
                case CashItemRequest.UpdatePetExceptionList:
                    break;
                case CashItemRequest.FreeCashItem:
                    break;
                case CashItemRequest.LoadFreeCashItem:
                    break;
                case CashItemRequest.Script:
                    break;
                case CashItemRequest.PurchaseRecord:
                    break;
                case CashItemRequest.TradeDone:
                    break;
                case CashItemRequest.BuyDone:
                    break;
                case CashItemRequest.TradeSave:
                    break;
                case CashItemRequest.TradeLog:
                    break;
                case CashItemRequest.EvolPet:
                    break;
                case CashItemRequest.BuyNameChange:
                    break;
                case CashItemRequest.CancelChangeName:
                    break;
                case CashItemRequest.BuyTransferWorld:
                    break;
                case CashItemRequest.CancelTransferWorld:
                    break;
                case CashItemRequest.CharacterSale:
                    break;
                case CashItemRequest.ItemUpgrade:
                    break;
                case CashItemRequest.ItemUpgradeFail:
                    break;
                case CashItemRequest.ItemUpgradeReq:
                    break;
                case CashItemRequest.ItemUpgradeDone:
                    break;
                case CashItemRequest.Vega:
                    break;
                case CashItemRequest.CashItemGachapon:
                    break;
                case CashItemRequest.CashGachaponOpen:
                    break;
                case CashItemRequest.CashGachaponCopy:
                    break;
                case CashItemRequest.ChangeMaplePoint:
                    break;
                case CashItemRequest.CheckFreeCashItemTable:
                    break;
                case CashItemRequest.SetFreeCashItemTable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}