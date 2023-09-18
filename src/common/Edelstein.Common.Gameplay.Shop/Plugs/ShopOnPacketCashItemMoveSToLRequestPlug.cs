using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class ShopOnPacketCashItemMoveSToLRequestPlug : IPipelinePlug<ShopOnPacketCashItemMoveSToLRequest>
{
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemMoveSToLRequest message)
    {
        if (message.User.Character == null) return;
        if (message.User.AccountWorld == null) return;
        if (message.User.AccountWorld.Locker.Items.Count >= message.User.AccountWorld.Locker.SlotMax) return;

        var inventory = message.User.Character.Inventories[message.Type];

        if (inventory == null) return;
        
        var (slot, item) = inventory.Items
            .FirstOrDefault(kv => (kv.Value as IItemSlotBase)?.CashItemSN == message.CashItemSN);

        if (item is not IItemSlotBase itemBase) return;
        if (itemBase.CashItemSN != message.CashItemSN) return;
        
        var context = new ModifyInventoryContext(message.Type, inventory, message.User.Context.Templates.Item);
        var lockerSlot = new ItemLockerSlot
        {
            Item = item
        };
        
        context.RemoveSlot(slot);
        message.User.AccountWorld.Locker.Items.Add(lockerSlot);
        
        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        p.WriteByte((byte)ShopResultOperations.MoveStoL_Done);
        p.WriteItemLockerData(lockerSlot);
        await message.User.Dispatch(p.Build());
    }
}
