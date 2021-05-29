using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify
{
    public interface IModifyInventoryContext
    {
        void Add(AbstractItemSlot item);
        void Add(int templateID, short quantity = 1);
        void Add(ItemTemplate template, short quantity = 1); // TODO: item variation

        void Set(short slot, AbstractItemSlot item);
        void Set(short slot, int templateID, short quantity = 1);
        void Set(short slot, ItemTemplate template, short quantity = 1);

        void Remove(short slot);
        void Remove(short slot, short count);
        void Remove(int templateID, short count);
        void Remove(AbstractItemSlot item);
        void Remove(AbstractItemSlot item, short count);

        void Move(short from, short to);

        ItemSlotBundle Take(short slot, short count = 1);
        ItemSlotBundle Take(int templateID, short count = 1);
        ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1);

        void Update(short slot);
        void Update(AbstractItemSlot item);

        IEnumerable<IModifyInventoryOperation> History();
        void Encode(IPacketWriter writer);
    }
}
