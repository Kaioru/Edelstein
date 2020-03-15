using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Trade.Handlers
{
    public class MigrateInHandler : AbstractPacketHandler<TradeServiceAdapter>
    {
        protected override async Task Handle(
            TradeServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var characterID = packet.Decode<int>();

            packet.Decode<long>(); // MachineID 1
            packet.Decode<long>(); // MachineID 2

            packet.Decode<bool>(); // isUserGM
            packet.Decode<byte>(); // Unk

            var clientKey = packet.Decode<long>();

            try
            {
                await adapter.TryMigrateFrom(characterID, clientKey);

                using var p = new Packet(SendPacketOperations.SetITC);

                adapter.Character.EncodeData(p);

                p.Encode<string>(adapter.Account.Username); // m_sNexonClubID

                p.Encode<int>(adapter.Service.State.RegisterFeeMeso);
                p.Encode<int>(adapter.Service.State.CommissionRate);
                p.Encode<int>(adapter.Service.State.CommissionBase);
                p.Encode<int>(adapter.Service.State.AuctionDurationMin);
                p.Encode<int>(adapter.Service.State.AuctionDurationMax);
                p.Encode<long>(0);

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