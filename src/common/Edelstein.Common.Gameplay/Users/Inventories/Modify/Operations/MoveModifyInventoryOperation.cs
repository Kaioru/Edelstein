using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations
{
    public class MoveModifyInventoryOperation : AbstractModifyInventoryOperation
    {
        public override ModifyInventoryOperations Operation => ModifyInventoryOperations.Move;

        public short ToSlot { get; init; }

        public MoveModifyInventoryOperation(
            ItemInventoryType type,
            short slot,
            short toSlot
        )
        {
            Type = type;
            Slot = slot;
            ToSlot = toSlot;
        }

        protected override void WriteData(IPacketWriter writer)
            => writer.WriteShort(ToSlot);
    }
}
