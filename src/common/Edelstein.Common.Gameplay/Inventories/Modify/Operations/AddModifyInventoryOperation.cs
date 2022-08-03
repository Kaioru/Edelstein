using Edelstein.Common.Gameplay.Inventories.Items;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Inventories.Modify.Operations;

public class AddModifyInventoryOperation : AbstractModifyInventoryOperation
{
    private readonly IItemSlot _item;

    public AddModifyInventoryOperation(
        ItemInventoryType inventory,
        short slot,
        IItemSlot item
    ) : base(ModifyInventoryOperationType.Add, inventory, slot) =>
        _item = item;

    protected override void WriteData(IPacketWriter writer) =>
        writer.WriteItemData(_item);
}
