using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class ShopOnPacketCashItemIncSlotCountRequestPlug : IPipelinePlug<ShopOnPacketCashItemIncSlotCountRequest>
{
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemIncSlotCountRequest message)
    {
        var inventory = message.User.Character?.Inventories[message.Type];
        
        if (!message.User.CheckCash(message.Cash, 4000)) return;
        if (inventory == null) return;
        if (inventory.SlotMax + 4 > 96) return;
        
        inventory.SlotMax += 4;
        message.User.IncCash(message.Cash, -4000);
        
        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);
    
        p.WriteByte((byte)ShopResultOperations.IncSlotCount_Done);
        p.WriteByte((byte)message.Type);
        p.WriteShort(inventory.SlotMax);
        await message.User.Dispatch(p.Build());
    }
}
