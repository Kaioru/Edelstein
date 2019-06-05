using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public abstract class AbstractItemUseHandler<T> : AbstractFieldUserHandler
        where T : ItemTemplate
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var position = packet.Decode<short>();
            var templateID = packet.Decode<int>();
            var template = user.Service.TemplateManager.Get<ItemTemplate>(templateID);

            if (template == null) return;
            if (!(template is T castTemplate)) return;

            var inventory = user.Character.Inventories[(ItemInventoryType) (templateID / 1000000)];
            var item = inventory.Items[position];

            if (item.TemplateID != templateID) return;

            await Handle(operation, packet, user, castTemplate, item);
            await user.ModifyInventory(i => i.Remove(item, 1), true);
        }

        public abstract Task Handle(
            RecvPacketOperations operation,
            IPacket packet,
            FieldUser user,
            T template,
            ItemSlot item
        );
    }
}