using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Edelstein.Service.Shop.Sockets
{
    public partial class WvsShopSocket
    {
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
                    p.Encode<byte>(0);

                    // m_aBest 1080 Buffer
                    Enumerable.Repeat((byte) 0, 1080).ForEach(b => p.Encode<byte>(b));

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
                .OrderBy(g => g.ID)
                .FirstOrDefault();

            if (service != null &&
                !await WvsShop.TryMigrateTo(this, Character, service))
                await Disconnect();
        }
    }
}