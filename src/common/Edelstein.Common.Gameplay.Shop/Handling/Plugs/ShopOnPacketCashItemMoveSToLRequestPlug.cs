using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Inventories;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

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
        
        using var packet = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        packet.WriteByte((byte)ShopResultOperations.MoveStoL_Done);
        packet.WriteItemLockerData(lockerSlot);
        await message.User.Dispatch(packet.Build());
    }
}
