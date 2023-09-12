using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;

public class MoveModifyInventoryOperation : AbstractModifyInventoryOperation
{
    public short ToSlot { get; }

    public MoveModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot,
        short toSlot
    ) : base(ModifyInventoryOperationType.Move, inventory, slot) =>
        ToSlot = toSlot;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteShort(ToSlot);
}
