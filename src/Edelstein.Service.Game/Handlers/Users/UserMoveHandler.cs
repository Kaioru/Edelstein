using System.Threading.Tasks;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Movements;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserMoveHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            packet.DecodeLong();
            packet.DecodeByte();
            packet.DecodeLong();
            packet.DecodeInt();
            packet.DecodeInt();
            packet.DecodeInt();

            var path = new MovePath(packet);

            using var p = new OutPacket(SendPacketOperations.UserMove);

            p.EncodeInt(user.ID);
            path.Encode(p);

            await user.BroadcastPacket(p);
            await user.Move(path);
        }
    }
}