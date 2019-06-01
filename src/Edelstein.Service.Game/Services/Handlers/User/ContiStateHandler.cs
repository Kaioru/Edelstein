using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class ContiStateHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var continent = user.Service.ContinentManager.Continents
                .FirstOrDefault(c => c.StartShipMoveField == user.Field ||
                                     c.MoveField == user.Field);

            if (continent == null) return;

            using (var p = new Packet(SendPacketOperations.CONTISTATE))
            {
                p.Encode<byte>((byte) continent.State);
                p.Encode<bool>(continent.EventDoing);
                await user.SendPacket(p);
            }
        }
    }
}