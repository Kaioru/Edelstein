using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserTransferFieldRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<byte>();

            var fieldID = packet.Decode<int>();

            if (fieldID != -1)
            {
                var field = user.Service.FieldManager.Get(fieldID);

                await field.Enter(user, 0);
                return;
            }

            var portalName = packet.Decode<string>();
            var portal = user.Field.GetPortal(portalName);

            await portal.Enter(user);
        }
    }
}