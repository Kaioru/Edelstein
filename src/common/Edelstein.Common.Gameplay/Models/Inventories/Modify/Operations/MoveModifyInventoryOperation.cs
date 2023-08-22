using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;

public class MoveModifyInventoryOperation : AbstractModifyInventoryOperation
{
    private readonly short _toSlot;

    public MoveModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot,
        short toSlot
    ) : base(ModifyInventoryOperationType.Move, inventory, slot) =>
        _toSlot = toSlot;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteShort(_toSlot);
}
