using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Inventories.Operations
{
    public class UpdateEXPInventoryOperation : AbstractInventoryOperation
    {
        protected override InventoryOperationType Type => InventoryOperationType.UpdateEXP;
        private readonly int _exp;

        public UpdateEXPInventoryOperation(
            ItemInventoryType inventory,
            short slot,
            int exp
        ) : base(inventory, slot)
            => _exp = exp;

        public override void EncodeData(IPacket packet)
            => packet.Encode<int>(_exp);
    }
}