using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Templates.Server.Shop;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using MoreLinq.Extensions;

namespace Edelstein.Service.Shop.Handlers
{
    public class MigrateInHandler : AbstractPacketHandler<ShopServiceAdapter>
    {
        protected override async Task Handle(
            ShopServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var characterID = packet.DecodeInt();

            packet.DecodeLong(); // MachineID 1
            packet.DecodeLong(); // MachineID 2

            packet.DecodeBool(); // isUserGM
            packet.DecodeByte(); // Unk

            var clientKey = packet.DecodeLong();

            try
            {
                await adapter.TryMigrateFrom(characterID, clientKey);

                using var p = new OutPacket(SendPacketOperations.SetCashShop);

                adapter.Character.EncodeData(p);

                p.EncodeBool(true); // m_bCashShopAuthorized
                p.EncodeString(adapter.Account.Username); // m_sNexonClubID

                var templates = adapter.Service.TemplateManager;

                var notSales = templates.GetAll<NotSaleTemplate>().ToList();
                p.EncodeInt(notSales.Count);
                notSales.ForEach(n => p.EncodeInt(n.ID));

                var modifiedCommodities = templates.GetAll<ModifiedCommodityTemplate>().ToList();
                p.EncodeShort((short) modifiedCommodities.Count);
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

                    p.EncodeInt(m.ID);
                    p.EncodeInt(flag);

                    if ((flag & 0x1) != 0) p.EncodeInt(m.ItemID.Value);
                    if ((flag & 0x2) != 0) p.EncodeShort(m.Count.Value);
                    if ((flag & 0x10) != 0) p.EncodeByte(m.Priority.Value);
                    if ((flag & 0x4) != 0) p.EncodeInt(m.Price.Value);
                    if ((flag & 0x8) != 0) p.EncodeByte(m.Bonus.Value);
                    if ((flag & 0x20) != 0) p.EncodeShort(m.Period.Value);
                    if ((flag & 0x20000) != 0) p.EncodeShort(m.ReqPOP.Value);
                    if ((flag & 0x40000) != 0) p.EncodeShort(m.ReqLEV.Value);
                    if ((flag & 0x40) != 0) p.EncodeInt(m.MaplePoint.Value);
                    if ((flag & 0x80) != 0) p.EncodeInt(m.Meso.Value);
                    if ((flag & 0x100) != 0) p.EncodeBool(m.ForPremiumUser.Value);
                    if ((flag & 0x200) != 0) p.EncodeByte(m.Gender.Value);
                    if ((flag & 0x400) != 0) p.EncodeBool(m.OnSale.Value);
                    if ((flag & 0x800) != 0) p.EncodeByte(m.Class.Value);
                    if ((flag & 0x1000) != 0) p.EncodeByte(m.Limit.Value);
                    if ((flag & 0x2000) != 0) p.EncodeShort(m.PbCash.Value);
                    if ((flag & 0x4000) != 0) p.EncodeShort(m.PbPoint.Value);
                    if ((flag & 0x8000) != 0) p.EncodeShort(m.PbGift.Value);
                    if ((flag & 0x10000) == 0) return;
                    p.EncodeByte((byte) m.PackageSN.Length);
                    m.PackageSN.ForEach(sn => p.EncodeInt(sn));
                });

                var categoryDiscounts = templates.GetAll<CategoryDiscountTemplate>().ToList();
                p.EncodeByte((byte) categoryDiscounts.Count);
                categoryDiscounts.ForEach(d =>
                {
                    p.EncodeByte(d.Category);
                    p.EncodeByte(d.CategorySub);
                    p.EncodeByte(d.DiscountRate);
                });

                const int bestLimit = 90;
                var best = templates.GetAll<BestTemplate>().ToList();
                best.Take(bestLimit).ForEach(b =>
                {
                    p.EncodeInt(b.Category);
                    p.EncodeInt(b.Gender);
                    p.EncodeInt(b.CommoditySN);
                });
                if (best.Count < bestLimit)
                {
                    Enumerable.Repeat(0, bestLimit - best.Count).ForEach(i =>
                    {
                        p.EncodeInt(0);
                        p.EncodeInt(0);
                        p.EncodeInt(0);
                    });
                }

                // DecodeStock
                p.EncodeShort(0);
                // DecodeLimitGoods
                p.EncodeShort(0);
                // DecodeZeroGoods
                p.EncodeShort(0);

                p.EncodeBool(false); // m_bEventOn
                p.EncodeInt(0);

                await adapter.SendPacket(p);

                adapter.Party = await adapter.Service.PartyManager.Load(adapter.Character);
                adapter.Guild = await adapter.Service.GuildManager.Load(adapter.Character);

                if (adapter.Party != null)
                    await adapter.Party.UpdateUserMigration(adapter.Character.ID, -1, -1);
            }
            catch
            {
                await adapter.Close();
            }
        }
    }
}