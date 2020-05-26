using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations.States;
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
            IPacketDecoder packet
        )
        {
            var channelID = packet.DecodeByte();

            // TODO: checks

            try
            {
                var service = (await user.Service.GetPeers())
                    .Select(n => n.State)
                    .OfType<GameNodeState>()
                    .Where(s => s.Worlds.Contains(user.AccountWorld.WorldID))
                    .OrderBy(s => s.ChannelID)
                    .ToImmutableList()[channelID];

                await user.Adapter.TryMigrateTo(service);
            }
            catch
            {
                using var p = new OutPacket(SendPacketOperations.TransferChannelReqIgnored);
                p.EncodeByte(0x1);
                await user.SendPacket(p);
            }
        }
    }
}