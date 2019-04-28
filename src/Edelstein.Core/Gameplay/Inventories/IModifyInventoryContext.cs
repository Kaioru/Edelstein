using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;

namespace Edelstein.Core.Gameplay.Inventories
{
    public interface IModifyInventoryContext
    {
        void Add(ItemSlot item);
        void Add(ItemTemplate template, short quantity = 1);

        void Set(short slot, ItemSlot item);
        void Set(short slot, ItemTemplate item, short quantity = 1);

        void Remove(short slot);
        void Remove(short slot, short count);
        void Remove(ItemSlot item);
        void Remove(int template, short count);

        void Move(short from, short to);

        ItemSlotBundle Take(short slot, short count = 1);
        ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1);
        ItemSlotBundle Take(int template, short count = 1);

        void Update(short slot);
        void Update(ItemSlot item);

        void Encode(IPacket packet);
    }
}