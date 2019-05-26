using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserActivatePetRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var position = packet.Decode<short>();
            var leader = packet.Decode<bool>();

            if (!user.Character.Inventories[ItemInventoryType.Cash].Items.ContainsKey(position)) return;

            var item = (ItemSlotPet) user.Character
                .Inventories[ItemInventoryType.Cash]
                .Items[position];
            var pet = user.Pets.FirstOrDefault(p => p.Item == item);
            // TODO: Follow the leader checks

            if (pet != null)
            {
                var id = pet.IDx;

                user.Pets.Remove(pet);
                user.Pets
                    .Where(p => p.IDx > id)
                    .ForEach(p => p.IDx--);
                await user.Field.BroadcastPacket(pet.GetLeaveFieldPacket());
            }
            else
            {
                if (user.Pets.Count >= 3 || !pet.Item.CashItemSN.HasValue)
                {
                    await user.ModifyStats(exclRequest: true);
                    return;
                }

                var id = leader ? 0 : user.Pets.Count;

                pet = new FieldUserPet(user, item, (byte) id);
                user.Pets
                    .Where(p => p.IDx >= id)
                    .ForEach(p => p.IDx++);
                user.Pets.Add(pet);
                await user.Field.BroadcastPacket(pet.GetEnterFieldPacket());
            }

            await user.ModifyStats(s =>
            {
                s.Pet1 = user.Pets.FirstOrDefault(p => p.IDx == 0)?.Item.CashItemSN ?? 0;
                s.Pet2 = user.Pets.FirstOrDefault(p => p.IDx == 2)?.Item.CashItemSN ?? 0;
                s.Pet3 = user.Pets.FirstOrDefault(p => p.IDx == 3)?.Item.CashItemSN ?? 0;
            }, true);
        }
    }
}