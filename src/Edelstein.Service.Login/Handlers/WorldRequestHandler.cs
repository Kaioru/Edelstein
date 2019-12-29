using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
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
            IPacket packet
        )
        {
            if (adapter.Account == null) return;

            await Task.WhenAll(adapter.Service.State.Worlds
                .Select(async w =>
                {
                    using var p = new Packet(SendPacketOperations.WorldInformation);

                    p.Encode<byte>(w.ID);
                    p.Encode<string>(w.Name);
                    p.Encode<byte>(w.State);
                    p.Encode<string>(w.EventDesc);
                    p.Encode<short>(w.EventEXP);
                    p.Encode<short>(w.EventDrop);
                    p.Encode<bool>(w.BlockCharCreation);

                    var services = (await adapter.Service.GetPeers())
                        .Select(n => n.State)
                        .OfType<GameServiceState>()
                        .Where(s => s.Worlds.Contains(w.ID))
                        .OrderBy(s => s.ChannelID)
                        .ToImmutableList();

                    p.Encode<byte>((byte) services.Count);
                    services.ForEach(s =>
                    {
                        p.Encode<string>(s.Name);
                        p.Encode<int>(0); // TODO: UserNo
                        p.Encode<byte>(w.ID);
                        p.Encode<byte>(s.ChannelID);
                        p.Encode<bool>(false); // TODO: AdultChannel
                    });

                    p.Encode<short>(0); // TODO: Balloon
                    await adapter.SendPacket(p);
                }));

            using (var p = new Packet(SendPacketOperations.WorldInformation))
            {
                p.Encode<byte>(0xFF);
                await adapter.SendPacket(p);
            }

            using (var p = new Packet(SendPacketOperations.LatestConnectedWorld))
            {
                p.Encode<int>(adapter.Account.LatestConnectedWorld);
                await adapter.SendPacket(p);
            }
        }
    }
}