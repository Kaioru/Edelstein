using System.Threading.Tasks;
using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public abstract class AbstractUseItemHandler<TTemplate> : AbstractFieldUserHandler
        where TTemplate : ItemTemplate
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            packet.DecodeInt();
            var position = packet.DecodeShort();
            var templateID = packet.DecodeInt();
            var template = user.Service.TemplateManager.Get<ItemTemplate>(templateID);

            if (template == null) return;
            if (!(template is TTemplate castTemplate)) return;

            var inventory = user.Character.Inventories[(ItemInventoryType) (templateID / 1000000)];
            var item = inventory.Items[position];

            if (item.TemplateID != templateID) return;
            if (await Handle(user, item, castTemplate, operation, packet))
                await user.ModifyInventory(i => i.Remove(item, 1), true);
            else
                await user.ModifyInventory(exclRequest: true);
        }

        protected abstract Task<bool> Handle(
            FieldUser user,
            ItemSlot item,
            TTemplate template,
            RecvPacketOperations operation,
            IPacketDecoder packet
        );
    }
}