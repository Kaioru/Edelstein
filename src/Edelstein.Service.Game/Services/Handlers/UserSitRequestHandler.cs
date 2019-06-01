using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserSitRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var id = packet.Decode<short>();

            using (var p = new Packet(SendPacketOperations.UserSitResult))
            {
                if (id < 0)
                {
                    user.PortableChairID = null;
                    p.Encode<byte>(0);
                }
                else
                {
                    p.Encode<byte>(1);
                    p.Encode<short>(id); // TODO: proper checks for this
                }

                await user.SendPacket(p);
            }

            if (user.PortableChairID != null) return;

            using (var p = new Packet(SendPacketOperations.UserSetActivePortableChair))
            {
                p.Encode<int>(user.ID);
                p.Encode<int>(0);
                await user.Field.BroadcastPacket(user, p);
            }
        }
    }
}