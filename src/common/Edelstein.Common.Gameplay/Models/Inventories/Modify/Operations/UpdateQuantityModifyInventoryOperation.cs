using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;

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
