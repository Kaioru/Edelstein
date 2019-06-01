using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserTransferChannelRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var channel = packet.Decode<byte>();

            try
            {
                var service = user.Service.Peers
                    .OfType<GameServiceInfo>()
                    .Where(g => g.WorldID == user.Service.Info.WorldID)
                    .OrderBy(g => g.ID)
                    .ToList()[channel];

                await user.Socket.TryMigrateTo(user.Account, user.Character, service);
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
                {
                    p.Encode<byte>(0x1);
                    await user.SendPacket(p);
                }
            }
        }
    }
}