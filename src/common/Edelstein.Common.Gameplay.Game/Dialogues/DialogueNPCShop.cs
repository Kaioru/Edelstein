using Edelstein.Common.Constants;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Dialogues;

public class DialogueNPCShop : IDialogueNPCShop
{
    public DialogueNPCShop(INPCShop shop) => Shop = shop;

    public INPCShop Shop { get; }
    
    public async Task<bool> HandleEnter(IFieldUser user)
    {
        using var packet = new PacketWriter(PacketSendOperations.OpenShopDlg);

        packet.WriteInt(Shop.ID);
        packet.WriteShort((short)Shop.Items.Count);
        
        foreach (var item in Shop.Items)
        {
            packet.WriteInt(item.TemplateID);
            
            packet.WriteInt(item.Price);
            packet.WriteByte(item.DiscountRate);
            
            packet.WriteInt(item.TokenTemplateID);
            packet.WriteInt(item.TokenPrice);
            
            packet.WriteInt(item.ItemPeriod);
            packet.WriteInt(item.LevelLimited);

            if (ItemConstants.IsRechargeableItem(item.TemplateID))
                packet.WriteDouble(item.UnitPrice);
            else
                packet.WriteShort((short)item.Quantity);

            packet.WriteShort(item.MaxPerSlot);
        }

        await user.Dispatch(packet.Build());
        return true;
    }
    public Task<bool> HandleLeave(IFieldUser user) => Task.FromResult(true);
}
