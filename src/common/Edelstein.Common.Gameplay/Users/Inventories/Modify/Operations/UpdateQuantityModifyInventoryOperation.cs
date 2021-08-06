using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations
{
    public class UpdateQuantityModifyInventoryOperation : AbstractModifyInventoryOperation
    {
        public override ModifyInventoryOperations Operation => ModifyInventoryOperations.UpdateQuantity;

        public short Quantity { get; init; }

        public UpdateQuantityModifyInventoryOperation(
            ItemInventoryType type,
            short slot,
            short quantity
        )
        {
            Type = type;
            Slot = slot;
            Quantity = quantity;
        }

        public override void WriteData(IPacketWriter writer)
            => writer.WriteShort(Quantity);
    }
}
