using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserTransferChannelRequestHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var channelID = packet.Decode<byte>();

            // TODO: checks

            try
            {
                var service = (await user.Service.GetPeers())
                    .Select(n => n.State)
                    .OfType<GameServiceState>()
                    .Where(s => s.Worlds.Contains(user.AccountWorld.WorldID))
                    .OrderBy(s => s.ChannelID)
                    .ToImmutableList()[channelID];

                await user.Adapter.TryMigrateTo(service);
            }
            catch
            {
                using var p = new Packet(SendPacketOperations.TransferChannelReqIgnored);
                p.Encode<byte>(0x1);
                await user.SendPacket(p);
            }
        }
    }
}