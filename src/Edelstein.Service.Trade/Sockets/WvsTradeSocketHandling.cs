using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packet;
using Edelstein.Service.Trade.Logging;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;

namespace Edelstein.Service.Trade.Sockets
{
    public partial class WvsTradeSocket
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

            using (var db = WvsTrade.DataContextFactory.Create())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Single(c => c.ID == characterID);

                if (!await WvsTrade.TryMigrateFrom(character, WvsTrade.Info))
                    await Disconnect();
                Character = character;

                using (var p = new Packet(SendPacketOperations.SetITC))
                {
                    character.EncodeData(p);

                    p.Encode<string>(character.Data.Account.Username); // m_sNexonClubID

                    p.Encode<int>(WvsTrade.Info.RegisterFeeMeso);
                    p.Encode<int>(WvsTrade.Info.CommissionRate);
                    p.Encode<int>(WvsTrade.Info.CommissionBase);
                    p.Encode<int>(WvsTrade.Info.AuctionDurationMin);
                    p.Encode<int>(WvsTrade.Info.AuctionDurationMax);
                    p.Encode<long>(0);
                    await SendPacket(p);
                }
            }
        }
        
        private async Task OnTransferFieldRequest(IPacket packet)
        {
            var service = WvsTrade.Peers
                .OfType<GameServiceInfo>()
                .Where(g => g.WorldID == Character.Data.WorldID)
                .FirstOrDefault(g => g.Name == Character.Data.Account.PreviousConnectedService);

            if (service != null &&
                !await WvsTrade.TryMigrateTo(this, Character, service))
                await Disconnect();
        }
    }
}