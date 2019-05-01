using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Extensions;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
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

                        // Not Sale
                        p.Encode<int>(0);

                        // Modified Commodity
                        p.Encode<short>(0);

                        // Category Discounts
                        p.Encode<byte>(0);

                        // Best
                        Enumerable.Repeat(0, 90).ForEach(i =>
                        {
                            p.Encode<int>(0);
                            p.Encode<int>(0);
                            p.Encode<int>(0);
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
    }
}