using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Templates.Items;

namespace Edelstein.Core.Gameplay.Inventories
{
    public interface IModifyInventoriesContext
    {
        IModifyInventoryContext this[ItemInventoryType key] { get; }

        void Add(ItemSlot slot);
        void Add(ItemTemplate template, short quantity = 1);

        void Set(short slot, ItemSlot item);
        void Set(short slot, ItemTemplate template, short quantity = 1);
        void Set(BodyPart part, ItemSlot item);
        void Set(BodyPart part, ItemTemplate template, short quantity = 1);

        void Remove(ItemSlot slot);
        void Remove(ItemSlot item, short count);
        void Remove(int template, short count);

        ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1);
        ItemSlotBundle Take(int template, short count = 1);

        void Update(ItemSlot slot);

        void Encode(IPacketEncoder packet);
    }
}