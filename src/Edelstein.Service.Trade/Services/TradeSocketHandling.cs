using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Trade.Services
{
    public partial class TradeSocket
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

                    Account = account;
                    AccountData = data;
                    Character = character;

                    await TryMigrateFrom(account, character);

                    if (SocialService != null)
                        await Service.SendMessage(SocialService, new SocialStateMessage
                        {
                            CharacterID = character.ID,
                            State = MigrationState.LoggedIn,
                            Service = Service.Info.Name
                        });

                    using (var p = new Packet(SendPacketOperations.SetITC))
                    {
                        character.EncodeData(p);

                        p.Encode<string>(Account.Username); // m_sNexonClubID

                        p.Encode<int>(Service.Info.RegisterFeeMeso);
                        p.Encode<int>(Service.Info.CommissionRate);
                        p.Encode<int>(Service.Info.CommissionBase);
                        p.Encode<int>(Service.Info.AuctionDurationMin);
                        p.Encode<int>(Service.Info.AuctionDurationMax);
                        p.Encode<long>(0);
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