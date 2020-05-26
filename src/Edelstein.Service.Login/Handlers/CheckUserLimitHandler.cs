using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations.States;
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
            IPacketDecoder packet
        )
        {
            var worldID = packet.DecodeByte();

            if (adapter.Account == null) return;

            var services = (await adapter.Service.GetPeers())
                .Select(n => n.State)
                .OfType<GameNodeState>()
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
                .First(w => w.ID == worldID)
                .UserLimit;
            var capacity = (double) userNo / Math.Max(1, userLimit);

            capacity = Math.Min(1, capacity);
            capacity = Math.Max(0, capacity);

            using var p = new OutPacket(SendPacketOperations.CheckUserLimitResult);

            p.EncodeByte(
                (byte) (capacity >= 1
                    ? 2
                    : capacity >= 0.75
                        ? 1
                        : 0)
            );
            p.EncodeByte(0); // TODO: bPopulateLevel

            await adapter.SendPacket(p);
        }
    }
}