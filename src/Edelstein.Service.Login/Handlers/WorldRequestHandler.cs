using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Gameplay.Migrations.States;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Handlers
{
    public class WorldRequestHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            if (adapter.Account == null) return;

            foreach (var w in adapter.Service.State.Worlds.OrderBy(w => w.ID))
            {
                using var p = new OutPacket(SendPacketOperations.WorldInformation);

                p.EncodeByte(w.ID);
                p.EncodeString(w.Name);
                p.EncodeByte(w.State);
                p.EncodeString(w.EventDesc);
                p.EncodeShort(w.EventEXP);
                p.EncodeShort(w.EventDrop);
                p.EncodeBool(w.BlockCharCreation);

                var services = (await adapter.Service.GetPeers())
                    .Select(n => n.State)
                    .OfType<GameNodeState>()
                    .Where(s => s.Worlds.Contains(w.ID))
                    .OrderBy(s => s.ChannelID)
                    .ToImmutableList();

                p.EncodeByte((byte) services.Count);
                foreach (var s in services)
                {
                    var userNo = await adapter.Service.SocketCountCache
                        .GetAsync<int>(s.Name);

                    p.EncodeString(s.Name);
                    p.EncodeInt(userNo.HasValue ? userNo.Value : 0);
                    p.EncodeByte(w.ID);
                    p.EncodeByte(s.ChannelID);
                    p.EncodeBool(false); // TODO: AdultChannel
                }

                p.EncodeShort(0); // TODO: Balloon
                await adapter.SendPacket(p);
            }

            using (var p = new OutPacket(SendPacketOperations.WorldInformation))
            {
                p.EncodeByte(0xFF);
                await adapter.SendPacket(p);
            }

            if (adapter.Account.LatestConnectedWorld == null) return;
            using (var p = new OutPacket(SendPacketOperations.LatestConnectedWorld))
            {
                p.EncodeInt(adapter.Account.LatestConnectedWorld ?? 0);
                await adapter.SendPacket(p);
            }
        }
    }
}