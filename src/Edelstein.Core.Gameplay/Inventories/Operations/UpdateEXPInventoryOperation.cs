using Edelstein.Core.Entities.Inventories;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Core.Gameplay.Inventories.Operations
{
    public class UpdateEXPInventoryOperation : AbstractModifyInventoryOperation
    {
        protected override ModifyInventoryOperationType Type => ModifyInventoryOperationType.UpdateEXP;
        private readonly int _exp;

        public UpdateEXPInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            int exp
        ) : base(inventory, slot)
        {
            _exp = exp;
        }

        protected override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeInt(_exp);
        }
    }
}