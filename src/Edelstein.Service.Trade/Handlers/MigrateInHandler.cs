using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;

namespace Edelstein.Service.Trade.Handlers
{
    public class MigrateInHandler : AbstractPacketHandler<TradeServiceAdapter>
    {
        protected override async Task Handle(
            TradeServiceAdapter adapter,
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

                using var p = new OutPacket(SendPacketOperations.SetITC);

                adapter.Character.EncodeData(p);

                p.EncodeString(adapter.Account.Username); // m_sNexonClubID

                p.EncodeInt(adapter.Service.State.RegisterFeeMeso);
                p.EncodeInt(adapter.Service.State.CommissionRate);
                p.EncodeInt(adapter.Service.State.CommissionBase);
                p.EncodeInt(adapter.Service.State.AuctionDurationMin);
                p.EncodeInt(adapter.Service.State.AuctionDurationMax);
                p.EncodeLong(0);

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