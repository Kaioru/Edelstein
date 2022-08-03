using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Inventories.Modify.Operations;

public class UpdateEXPModifyInventoryOperation : AbstractModifyInventoryOperation
{
    private readonly int _exp;

    public UpdateEXPModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot,
        int exp
    ) : base(ModifyInventoryOperationType.UpdateEXP, inventory, slot) =>
        _exp = exp;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteInt(_exp);
}
