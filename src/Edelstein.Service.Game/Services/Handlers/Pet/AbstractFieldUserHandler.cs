using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Services.Handlers.User;

namespace Edelstein.Service.Game.Services.Handlers.Pet
{
    public abstract class AbstractFieldUserPetHandler : AbstractFieldUserHandler
    {
        public override Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var sn = packet.Decode<long>();
            var pet = user.Pets.FirstOrDefault(p => p.Item.CashItemSN == sn);
            return pet == null
                ? Task.CompletedTask
                : Handle(operation, packet, pet);
        }

        public abstract Task Handle(RecvPacketOperations operation, IPacket packet, FieldUserPet pet);
    }
}