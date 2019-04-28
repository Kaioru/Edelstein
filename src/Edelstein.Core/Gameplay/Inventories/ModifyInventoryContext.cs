using System.Collections.Generic;
using Edelstein.Core.Gameplay.Inventories.Operations;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ModifyInventoryContext : IModifyInventoryContext
    {
        private readonly ItemInventoryType _type;
        private readonly ItemInventory _inventory;
        private readonly Queue<AbstractModifyInventoryOperation> _operations;

        public ModifyInventoryContext(ItemInventoryType type, ItemInventory inventory)
        {
            _type = type;
            _inventory = inventory;
            _operations = new Queue<AbstractModifyInventoryOperation>();
        }

        public void Add(ItemSlot item)
        {
            throw new System.NotImplementedException();
        }

        public void Add(ItemTemplate template, short quantity = 1)
        {
            throw new System.NotImplementedException();
        }

        public void Set(short slot, ItemSlot item)
        {
            throw new System.NotImplementedException();
        }

        public void Set(short slot, ItemTemplate item, short quantity = 1)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(short slot)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(short slot, short count)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(ItemSlot slot)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int template, short count)
        {
            throw new System.NotImplementedException();
        }

        public void Move(short @from, short to)
        {
            throw new System.NotImplementedException();
        }

        public ItemSlotBundle Take(short slot, short count = 1)
        {
            throw new System.NotImplementedException();
        }

        public ItemSlotBundle Take(ItemSlotBundle bundle, short count = 1)
        {
            throw new System.NotImplementedException();
        }

        public ItemSlotBundle Take(int template, short count = 1)
        {
            throw new System.NotImplementedException();
        }

        public void Update(short slot)
        {
            throw new System.NotImplementedException();
        }

        public void Update(ItemSlot slot)
        {
            throw new System.NotImplementedException();
        }

        public void Encode(IPacket packet)
        {
            throw new System.NotImplementedException();
        }
    }
}