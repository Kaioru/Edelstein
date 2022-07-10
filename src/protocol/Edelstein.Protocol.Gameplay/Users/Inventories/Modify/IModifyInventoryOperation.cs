using System;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Modify
{
    public interface IModifyInventoryOperation
    {
        ModifyInventoryOperations Operation { get; }

        ItemInventoryType Type { get; }
        short Slot { get; }

        DateTime Timestamp { get; }
    }
}
