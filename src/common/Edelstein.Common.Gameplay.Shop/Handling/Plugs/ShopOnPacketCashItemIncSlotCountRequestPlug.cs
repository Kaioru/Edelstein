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
        
        using var packet = new PacketWriter(PacketSendOperations.CashShopCashItemResult);
    
        packet.WriteByte((byte)ShopResultOperations.IncSlotCount_Done);
        packet.WriteByte((byte)message.Type);
        packet.WriteShort(inventory.SlotMax);
        await message.User.Dispatch(packet.Build());
    }
}
