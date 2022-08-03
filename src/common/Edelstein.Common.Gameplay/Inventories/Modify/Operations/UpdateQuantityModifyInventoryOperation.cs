using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Inventories.Modify.Operations;

public class UpdateQuantityModifyInventoryOperation : AbstractModifyInventoryOperation
{
    private readonly short _quantity;

    public UpdateQuantityModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot,
        short quantity
    ) : base(ModifyInventoryOperationType.UpdateQuantity, inventory, slot) =>
        _quantity = quantity;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteShort(_quantity);
}
