using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations
{
    public class AddModifyInventoryOperation : AbstractModifyInventoryOperation
    {
        public override ModifyInventoryOperations Operation => ModifyInventoryOperations.Add;

        public AbstractItemSlot Item { get; init; }

        public AddModifyInventoryOperation(
            ItemInventoryType type,
            short slot,
            AbstractItemSlot item
        )
        {
            Type = type;
            Slot = slot;
            Item = item;
        }

        protected override void WriteData(IPacketWriter writer)
            => writer.WriteItemData(Item);
    }
}
