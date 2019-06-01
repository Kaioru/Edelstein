using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Drop;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class DropPickupRequestHandler : AbstractFieldDropHandler
    {
        public override async Task Handle(
            RecvPacketOperations operation,
            IPacket packet,
            FieldUser user,
            AbstractFieldDrop drop
        )
        {
            await drop.PickUp(user);
        }
    }
}