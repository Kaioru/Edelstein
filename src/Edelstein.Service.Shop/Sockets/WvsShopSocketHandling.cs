using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Server.Best;
using Edelstein.Provider.Templates.Server.CategoryDiscount;
using Edelstein.Service.Shop.Logging;
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

                    // SetSaleInfo
                    p.Encode<int>(0);
                    p.Encode<short>(0);

                    var categoryDiscounts = WvsShop.TemplateManager.GetAll<CategoryDiscountTemplate>().ToList();
                    p.Encode<byte>((byte) categoryDiscounts.Count);
                    categoryDiscounts.ForEach(d =>
                    {
                        p.Encode<byte>(d.Category);
                        p.Encode<byte>(d.CategorySub);
                        p.Encode<byte>(d.DiscountRate);
                    });

                    // m_aBest 1080 Buffer
                    Enumerable.Repeat((byte) 0, 120).ForEach(b => p.Encode<byte>(b));
                    WvsShop.TemplateManager.GetAll<BestTemplate>()
                        .ForEach(b =>
                        {
                            p.Encode<int>(b.Category);
                            p.Encode<int>(b.Gender);
                            p.Encode<int>(b.CommoditySN);
                        });

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
    }
}