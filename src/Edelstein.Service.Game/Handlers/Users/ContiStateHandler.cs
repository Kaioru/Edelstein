using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class ContiStateHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var continent = user.Service.ContinentManager.All()
                .FirstOrDefault(c =>
                    c.StartShipMoveField == user.Field ||
                    c.MoveField == user.Field
                );

            if (continent == null) return;

            using var p = new Packet(SendPacketOperations.CONTISTATE);
            p.EncodeByte((byte) continent.State);
            p.EncodeBool(continent.EventDoing);
            await user.SendPacket(p);
        }
    }
}