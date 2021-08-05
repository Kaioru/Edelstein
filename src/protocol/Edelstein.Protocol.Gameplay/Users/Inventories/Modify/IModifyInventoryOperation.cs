using System;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Modify
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
