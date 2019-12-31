using System.Threading.Tasks;
using Edelstein.Core.Utils;
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
            IPacket packet
        )
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            var path = new MovePath(packet);

            using var p = new Packet(SendPacketOperations.UserMove);

            p.Encode<int>(user.ID);
            path.Encode(p);

            await user.BroadcastPacket(p);
            await user.Move(path);
        }
    }
}