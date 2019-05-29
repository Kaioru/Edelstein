using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.Pet
{
    public class PetMoveHandler : AbstractFieldUserPetHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUserPet pet)
        {
            using (var p = new Packet(SendPacketOperations.PetMove))
            {
                p.Encode<int>(pet.Owner.ID);
                p.Encode<byte>(pet.IDx);

                pet.Move(packet).Encode(p);

                await pet.Field.BroadcastPacket(pet.Owner, p);
            }
        }
    }
}