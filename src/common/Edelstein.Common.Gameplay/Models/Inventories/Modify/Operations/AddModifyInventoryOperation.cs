using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Inventories.Modify.Operations;

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
