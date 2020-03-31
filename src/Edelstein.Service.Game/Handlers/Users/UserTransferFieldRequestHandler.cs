using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserTransferFieldRequestHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            packet.DecodeByte();

            var fieldID = packet.DecodeInt();
            
            // TODO: proper checks

            if (fieldID != -1)
            {
                var field = user.Service.FieldManager.Get(fieldID);

                await field.Enter(user, 0);
                return;
            }

            var portalName = packet.DecodeString();
            var portal = user.Field.GetPortal(portalName);

            await portal.Enter(user);
        }
    }
}