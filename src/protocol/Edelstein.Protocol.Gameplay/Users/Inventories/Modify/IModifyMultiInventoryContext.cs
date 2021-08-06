using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Modify
{
    public interface IModifyMultiInventoryContext
    {
        IModifyInventoryContext this[ItemInventoryType key] { get; }

        void Add(AbstractItemSlot item);
        void Add(int templateID, short quantity = 1);
        void Add(ItemTemplate template, short quantity = 1); // TODO: item variation

        void Set(short slot, AbstractItemSlot item);
        void Set(short slot, int templateID, short quantity = 1);
        void Set(short slot, ItemTemplate template, short quantity = 1);

        void Set(BodyPart part, ItemSlotEquip equip);
        void Set(BodyPart part, int templateID);
        void Set(BodyPart part, ItemEquipTemplate template);

        void Remove(int templateID, short count);
        void Remove(AbstractItemSlot item);
        void Remove(AbstractItemSlot item, short count);

        ItemSlotBundle Take(int templateID, short count = 1);
        ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1);

        void Update(AbstractItemSlot item);

        IEnumerable<IModifyInventoryOperation> History();
    }
}
