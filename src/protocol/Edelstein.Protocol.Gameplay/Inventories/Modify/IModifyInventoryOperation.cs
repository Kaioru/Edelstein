using System;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Inventories.Modify
{
    public interface IModifyInventoryOperation
    {
        ModifyInventoryOperations Operation { get; }
        ItemInventoryType InventoryType { get; }
        short InventorySlot { get; }

        DateTime Timestamp { get; }

        void Encode(IPacketWriter writer);
    }
}
