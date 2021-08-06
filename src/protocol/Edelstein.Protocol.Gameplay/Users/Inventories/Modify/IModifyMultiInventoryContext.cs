using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Modify
{
    public interface IModifyMultiInventoryContext
    {
        IModifyInventoryContext this[ItemInventoryType key] { get; }
        IEnumerable<IModifyInventoryOperation> History { get; }

        void Set(BodyPart part, ItemSlotEquip equip);
        void Set(BodyPart part, int templateID);
        void Set(BodyPart part, ItemEquipTemplate template);

        void Add(AbstractItemSlot item);
        void Add(int templateID, short quantity = 1);
        void Add(ItemTemplate template, short quantity = 1); // TODO: item variation

        void Remove(int templateID, short count);
        void Remove(AbstractItemSlot item);
        void Remove(AbstractItemSlot item, short count);

        void Update(AbstractItemSlot item);
    }
}
