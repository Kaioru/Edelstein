using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify
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

        void Remove(int templateID, short count);
        void Remove(AbstractItemSlot item);
        void Remove(AbstractItemSlot item, short count);

        ItemSlotBundle Take(int templateID, short count = 1);
        ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1);

        void Update(AbstractItemSlot item);

        IEnumerable<IModifyInventoryOperation> History();
    }
}
