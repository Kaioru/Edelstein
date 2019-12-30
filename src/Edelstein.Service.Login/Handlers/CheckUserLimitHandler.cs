using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Handlers
{
    public class CheckUserLimitHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var worldID = packet.Decode<byte>();

            if (adapter.Account == null) return;

            var services = (await adapter.Service.GetPeers())
                .Select(n => n.State)
                .OfType<GameServiceState>()
                .Where(s => s.Worlds.Contains(worldID))
                .ToImmutableList();
            var tasks = services
                .Select(async s => await adapter.Service.SocketCountCache.GetAsync<int>(s.Name))
                .ToImmutableList();

            await Task.WhenAll(tasks);

            var userNo = tasks
                .Select(c => c.Result)
                .Select(r => r.HasValue ? r.Value : 0)
                .Sum();
            var userLimit = adapter.Service.State.Worlds
                                .FirstOrDefault(w => w.ID == worldID)
                                ?.UserLimit ?? 1000;
            var capacity = (double) userNo / Math.Max(1, userLimit);

            capacity = Math.Min(1, capacity);
            capacity = Math.Max(0, capacity);

            using var p = new Packet(SendPacketOperations.CheckUserLimitResult);

            p.Encode<byte>(
                (byte) (capacity >= 1
                    ? 2
                    : capacity >= 0.75
                        ? 1
                        : 0)
            );
            p.Encode<byte>(0); // TODO: bPopulateLevel

            await adapter.SendPacket(p);
        }
    }
}