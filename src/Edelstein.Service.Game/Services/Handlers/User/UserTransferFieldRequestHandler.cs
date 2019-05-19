using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserTransferFieldRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<byte>();
            packet.Decode<int>();

            var name = packet.Decode<string>();
            var portal = user.Field.GetPortal(name);

            await portal.Enter(user);
        }
    }
}