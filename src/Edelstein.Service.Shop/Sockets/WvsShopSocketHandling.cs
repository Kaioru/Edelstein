using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Constants;
using Edelstein.Core.Extensions;
using Edelstein.Core.Inventories;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities;
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
                    .Include(c => c.WishList)
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

                var gifts = db.GiftList
                    .Where(g => g.CharacterID == characterID)
                    .ToList();

                gifts
                    .Select(g => WvsShop.CommodityManager.Get(g.CommoditySN))
                    .Select(c =>
                    {
                        var template = WvsShop.TemplateManager.Get<ItemTemplate>(c.ItemID);
                        return c.ToItemSlot(template);
                    })
                    .ForEach(i => character.Data.Locker.Items.Add(i));
                db.GiftList.RemoveRange(gifts);
                db.SaveChanges();

                using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
                {
                    p.Encode<byte>((byte) CashItemResult.LoadGift_Done);
                    p.Encode<short>((short) gifts.Count);
                    gifts.ForEach(g =>
                    {
                        var commodity = WvsShop.CommodityManager.Get(g.CommoditySN);

                        p.Encode<long>(g.SN);
                        p.Encode<int>(commodity.ItemID);
                        p.EncodeFixedString(g.BuyCharacterName, 13);
                        p.EncodeFixedString(g.Text, 73);
                    });
                    await SendPacket(p);
                }

                await SendLockerData();
                await SendWishListData();
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

        public async Task SendLockerData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.LoadLocker_Done);

                var locker = Character.Data.Locker;

                p.Encode<short>((short) locker.Items.Count);
                locker.Items.ForEach(i => i.EncodeLocker(p));

                p.Encode<short>(Character.Data.Trunk.SlotMax);
                p.Encode<short>((short) Character.Data.SlotCount);
                p.Encode<short>(0);
                p.Encode<short>((short) Character.Data.Characters.Count);
                await SendPacket(p);
            }
        }

        public async Task SendWishListData()
        {
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.LoadWish_Done);

                var wishList = Character.WishList;

                wishList.ToList().ForEach(w =>
                {
                    var commodity = WvsShop.CommodityManager.Get(w.SN);

                    if (!commodity.OnSale)
                        wishList.Remove(w);
                });

                wishList.ForEach(w => p.Encode<int>(w.SN));
                if (wishList.Count < 10)
                    Enumerable.Repeat(0, 10 - wishList.Count).ForEach(i => p.Encode<int>(0));
                await SendPacket(p);
            }
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

        private Task OnCashShopQueryCashRequest(IPacket packet) => SendCashData();

        private Task OnCashShopCashItemRequest(IPacket packet)
        {
            var type = (CashItemRequest) packet.Decode<byte>();

            switch (type)
            {
                case CashItemRequest.Buy:
                    return OnBuy(packet);
                case CashItemRequest.SetWish:
                    return OnSetWish(packet);
                case CashItemRequest.IncCharSlotCount:
                    return OnIncCharSlotCount(packet);
                case CashItemRequest.MoveLtoS:
                    return OnMoveLtoS(packet);
                case CashItemRequest.MoveStoL:
                    return OnMoveStoL(packet);
                case CashItemRequest.BuyPackage:
                    return OnBuyPackage(packet);
                default:
                    Logger.Warn($"Unhandled cash item operation {type}");

                    using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
                    {
                        p.Encode<byte>((byte) CashItemResult.SetWish_Failed);
                        p.Encode<byte>(0);
                        return SendPacket(p);
                    }
            }
        }

        private async Task OnBuy(IPacket packet)
        {
            packet.Decode<byte>();
            var cashType = packet.Decode<int>();
            var commoditySN = packet.Decode<int>();
            var commodity = WvsShop.CommodityManager.Get(commoditySN);
            var account = Character.Data.Account;
            var locker = Character.Data.Locker;

            if (commodity == null) return;
            if (!commodity.OnSale) return;

            var category = commoditySN / 10000000;
            var categorySub = commoditySN / 100000 % 100;
            var discountRate = WvsShop.TemplateManager.GetAll<CategoryDiscountTemplate>()
                                   .FirstOrDefault(d => d.Category == category &&
                                                        d.CategorySub == categorySub)
                                   ?.DiscountRate ?? 0.0;
            var price = commodity.Price * (1 - discountRate / 100);

            if (account.GetCash(cashType) < price) return;
            if (locker.Items.Count >= locker.SlotMax) return;

            var template = WvsShop.TemplateManager.Get<ItemTemplate>(commodity.ItemID);
            var slot = commodity.ToItemSlot(template);

            locker.Items.Add(slot);

            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.Buy_Done);
                slot.EncodeLocker(p);
                await SendPacket(p);
            }

            account.IncCash(cashType, (int) -price);
            await SendCashData();
        }

        private async Task OnSetWish(IPacket packet)
        {
            var wishList = Character.WishList;

            wishList.Clear();
            for (var i = 0; i < 10; i++)
            {
                var sn = packet.Decode<int>();
                if (sn <= 0) continue;
                var commodity = WvsShop.CommodityManager.Get(sn);

                if (commodity.OnSale)
                    wishList.Add(new WishList {SN = sn});
            }

            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.SetWish_Done);
                wishList.ForEach(w => p.Encode<int>(w.SN));
                if (wishList.Count < 10)
                    Enumerable.Repeat(0, 10 - wishList.Count).ForEach(i => p.Encode<int>(0));
                await SendPacket(p);
            }
        }

        private async Task OnIncCharSlotCount(IPacket packet)
        {
            packet.Decode<byte>();
            var cashType = packet.Decode<int>();
            var commoditySN = packet.Decode<int>();
            var commodity = WvsShop.CommodityManager.Get(commoditySN);
            var account = Character.Data.Account;
            var data = Character.Data;

            if (commodity == null) return;
            if (!commodity.OnSale) return;

            var category = commoditySN / 10000000;
            var categorySub = commoditySN / 100000 % 100;
            var discountRate = WvsShop.TemplateManager.GetAll<CategoryDiscountTemplate>()
                                   .FirstOrDefault(d => d.Category == category &&
                                                        d.CategorySub == categorySub)
                                   ?.DiscountRate ?? 0.0;
            var price = commodity.Price * (1 - discountRate / 100);

            if (account.GetCash(cashType) < price) return;
            if (data.SlotCount >= 15) return;

            data.SlotCount++;

            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.IncBuyCharCount_Done);
                p.Encode<short>((short) data.SlotCount);
                await SendPacket(p);
            }

            account.IncCash(cashType, (int) -price);
            await SendCashData();
            await SendLockerData();
        }

        private async Task OnMoveLtoS(IPacket packet)
        {
            var sn = packet.Decode<long>();
            var locker = Character.Data.Locker;

            var item = locker.Items.FirstOrDefault(i => i.CashItemSN == sn);
            if (item == null) return;

            var context = new ModifyInventoryContext(Character);
            if (!Character.HasSlotFor(item)) return;
            item.ID = 0;
            item.ItemLocker = null;
            locker.Items.Remove(item);
            context.Add(item);
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.MoveLtoS_Done);
                p.Encode<short>(item.Position);
                item.Encode(p);
                await SendPacket(p);
            }
        }

        private async Task OnMoveStoL(IPacket packet)
        {
            var id = packet.Decode<long>();
            var inventories = Character.Inventories;

            var item = inventories
                .SelectMany(i => i.Items)
                .FirstOrDefault(i => i.CashItemSN == id);
            if (item?.CashItemSN == null) return;
            var context = new ModifyInventoryContext(Character);

            var locker = Character.Data.Locker;
            if (locker.Items.Count >= locker.SlotMax) return;
            context.Remove(item);
            locker.Items.Add(item);
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.MoveStoL_Done);
                item.EncodeLocker(p);
                await SendPacket(p);
            }
        }

        private async Task OnBuyPackage(IPacket packet)
        {
            packet.Decode<byte>();
            var cashType = packet.Decode<int>();
            var commoditySN = packet.Decode<int>();
            var commodity = WvsShop.CommodityManager.Get(commoditySN);
            var account = Character.Data.Account;

            var locker = Character.Data.Locker;
            if (commodity == null) return;
            if (!commodity.OnSale) return;
            var category = commoditySN / 10000000;
            var categorySub = commoditySN / 100000 % 100;

            var discountRate = WvsShop.TemplateManager.GetAll<CategoryDiscountTemplate>()
                                   .FirstOrDefault(d => d.Category == category &&
                                                        d.CategorySub == categorySub)
                                   ?.DiscountRate ?? 0.0;

            var price = commodity.Price * (1 - discountRate / 100);
            if (account.GetCash(cashType) < price) return;
            if (locker.Items.Count >= locker.SlotMax) return;

            var slots = commodity.PackageSN
                .Select(p => WvsShop.CommodityManager.Get(p))
                .Select(p => p.ToItemSlot(WvsShop.TemplateManager
                    .Get<ItemTemplate>(p.ItemID)))
                .ToList();

            slots.ForEach(s => locker.Items.Add(s));
            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.BuyPackage_Done);
                p.Encode<byte>((byte) slots.Count);
                slots.ForEach(s => s.EncodeLocker(p));
                p.Encode<short>(0); // MaplePoints stuff?
                await SendPacket(p);
            }

            account.IncCash(cashType, (int) -price);
            await SendCashData();
        }
    }
}