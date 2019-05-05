using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Extensions;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Core.Gameplay.Inventories;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Entities.Inventories.Cash;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Shop;
using Edelstein.Service.Shop.Extensions;
using Edelstein.Service.Shop.Logging;
using Edelstein.Service.Shop.Types;
using MoreLinq;

namespace Edelstein.Service.Shop.Services
{
    public partial class ShopSocket
    {
        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            try
            {
                using (var store = Service.DataStore.OpenSession())
                {
                    var character = store
                        .Query<Character>()
                        .First(c => c.ID == characterID);
                    var data = store
                        .Query<AccountData>()
                        .First(d => d.ID == character.AccountDataID);
                    var account = store
                        .Query<Account>()
                        .First(a => a.ID == data.AccountID);

                    await TryMigrateFrom(account, character);

                    Account = account;
                    AccountData = data;
                    Character = character;

                    using (var p = new Packet(SendPacketOperations.SetCashShop))
                    {
                        character.EncodeData(p);

                        p.Encode<bool>(true); // m_bCashShopAuthorized
                        p.Encode<string>(Account.Username); // m_sNexonClubID

                        var templates = Service.TemplateManager;

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
                    await SendWishListData();
                    await SendCashData();
                }
            }
            catch
            {
                await Close();
            }
        }

        private async Task OnUserTransferFieldRequest(IPacket packet)
        {
            try
            {
                var service = Service.Peers
                    .OfType<GameServiceInfo>()
                    .Where(g => g.WorldID == AccountData.WorldID)
                    .First(g => g.Name == Account.PreviousConnectedService);

                await TryMigrateTo(Account, Character, service);
            }
            catch
            {
                await Close();
            }
        }

        private Task OnCashShopQueryCashRequest(IPacket packet) => SendCashData();

        private Task OnCashShopCashItemRequest(IPacket packet)
        {
            var type = (CashItemRequest) packet.Decode<byte>();

            return type switch {
                CashItemRequest.Buy => OnBuy(packet),
                CashItemRequest.MoveLtoS => OnMoveLtoS(packet),
                CashItemRequest.MoveStoL => OnMoveStoL(packet),
                _ => Task.Run(() => Logger.Warn($"Unhandled cash item operation {type}"))
                };
        }

        private async Task OnBuy(IPacket packet)
        {
            packet.Decode<byte>();
            var cashType = packet.Decode<int>();
            var commoditySN = packet.Decode<int>();
            var commodity = Service.CommodityManager.Get(commoditySN);
            var locker = AccountData.Locker;

            if (commodity == null) return;
            if (!commodity.OnSale) return;

            var price = GetDiscountedPrice(commodity);

            if (Account.GetCash(cashType) < price) return;
            if (locker.Items.Count >= locker.SlotMax) return;

            var template = Service.TemplateManager.Get<ItemTemplate>(commodity.ItemID);
            var slot = new ItemLockerSlot(template.ToItemSlot());

            locker.Items.Add(slot);

            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.Buy_Done);
                slot.Encode(p);
                await SendPacket(p);
            }

            Account.IncCash(cashType, -price);
            await SendCashData();
        }

        private async Task OnMoveLtoS(IPacket packet)
        {
            var sn = packet.Decode<long>();
            var locker = AccountData.Locker;
            var slot = locker.Items.FirstOrDefault(i => i.Item.CashItemSN == sn);

            if (slot == null) return;
            if (!Character.HasSlotFor(slot.Item)) return;

            var context = new ModifyInventoriesContext(Character.Inventories);

            locker.Items.Remove(slot);
            context.Add(slot.Item);

            var position = Character.Inventories.Values
                .SelectMany(i => i.Items)
                .First(i => i.Value == slot.Item).Key;

            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.MoveLtoS_Done);
                p.Encode<short>(position);
                slot.Item.Encode(p);
                await SendPacket(p);
            }
        }

        private async Task OnMoveStoL(IPacket packet)
        {
            var id = packet.Decode<long>();
            var locker = AccountData.Locker;
            var inventories = Character.Inventories;
            var item = inventories.Values
                .SelectMany(i => i.Items.Values)
                .FirstOrDefault(i => i.CashItemSN == id);

            if (item == null) return;
            if (locker.Items.Count >= locker.SlotMax) return;

            var context = new ModifyInventoriesContext(Character.Inventories);
            var slot = new ItemLockerSlot(item);

            context.Remove(item);
            locker.Items.Add(slot);

            using (var p = new Packet(SendPacketOperations.CashShopCashItemResult))
            {
                p.Encode<byte>((byte) CashItemResult.MoveStoL_Done);
                slot.Encode(p);
                await SendPacket(p);
            }
        }
    }
}