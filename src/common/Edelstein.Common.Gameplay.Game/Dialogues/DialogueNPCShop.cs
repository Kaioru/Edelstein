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
        var p = new PacketWriter(PacketSendOperations.OpenShopDlg);

        p.WriteInt(Shop.ID);
        p.WriteShort((short)Shop.Items.Count);
        
        foreach (var item in Shop.Items)
        {
            p.WriteInt(item.TemplateID);
            
            p.WriteInt(item.Price);
            p.WriteByte(item.DiscountRate);
            
            p.WriteInt(item.TokenTemplateID);
            p.WriteInt(item.TokenPrice);
            
            p.WriteInt(item.ItemPeriod);
            p.WriteInt(item.LevelLimited);

            if (ItemConstants.IsRechargeableItem(item.TemplateID))
                p.WriteDouble(item.UnitPrice);
            else
                p.WriteShort((short)item.Quantity);

            p.WriteShort(item.MaxPerSlot);
        }

        await user.Dispatch(p.Build());
        return true;
    }
    public Task<bool> HandleLeave(IFieldUser user) => Task.FromResult(true);
}
