using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Users.Inventories.Modify.Operations
{
    public class UpdateEXPModifyInventoryOperation : AbstractModifyInventoryOperation
    {
        public override ModifyInventoryOperations Operation => ModifyInventoryOperations.UpdateEXP;

        public int EXP { get; init; }

        public UpdateEXPModifyInventoryOperation(
            ItemInventoryType type,
            short slot,
            int exp
        )
        {
            Type = type;
            Slot = slot;
            EXP = exp;
        }

        public override void WriteData(IPacketWriter writer)
            => writer.WriteInt(EXP);
    }
}
