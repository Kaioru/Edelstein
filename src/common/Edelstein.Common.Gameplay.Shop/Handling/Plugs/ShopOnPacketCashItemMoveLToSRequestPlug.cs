using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class ShopOnPacketCashItemMoveLToSRequestPlug : IPipelinePlug<ShopOnPacketCashItemMoveLToSRequest>
{
    private readonly IInventoryManager _inventoryManager;
    
    public ShopOnPacketCashItemMoveLToSRequestPlug(IInventoryManager inventoryManager) => _inventoryManager = inventoryManager;
    
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemMoveLToSRequest message)
    {
        if (message.User.Character == null) return;
        if (message.User.AccountWorld == null) return;
        
        var slot = message.User.AccountWorld.Locker.Items
            .FirstOrDefault(i => (i.Item as IItemSlotBase)?.CashItemSN == message.CashItemSN);
        var context = new ModifyInventoryGroupContext(message.User.Character.Inventories, message.User.Context.Templates.Item);

        if (slot == null) return;
        if (!_inventoryManager.HasSlotFor(message.User.Character.Inventories, slot.Item)) return;

        message.User.AccountWorld.Locker.Items.Remove(slot);
        var target = context.Add(slot.Item);

        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        p.WriteByte((byte)ShopResultOperations.MoveLtoS_Done);
        p.WriteShort(target);
        p.WriteItemData(slot.Item);
        await message.User.Dispatch(p.Build());
    }
}
