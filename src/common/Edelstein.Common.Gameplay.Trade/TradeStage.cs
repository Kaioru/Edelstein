using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Trade;

namespace Edelstein.Common.Gameplay.Trade;

public class TradeStage : AbstractStage<ITradeStageUser>, ITradeStage
{
    public TradeStage(string id) => ID = id;

    public override string ID { get; }

    public new async Task Enter(ITradeStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
        {
            await user.Disconnect();
            return;
        }
        
        var packet = new PacketWriter(PacketSendOperations.SetITC);
        
        packet.WriteCharacterData(
            user.Character, 
            DbFlags.Character | 
            DbFlags.Money | 
            DbFlags.ItemSlotEquip | 
            DbFlags.ItemSlotConsume | 
            DbFlags.ItemSlotInstall |
            DbFlags.ItemSlotEtc | 
            DbFlags.ItemSlotCash | 
            DbFlags.InventorySize
        );
        
        packet.WriteString(user.Account.Username);

        packet.WriteInt(user.Context.Options.RegisterFeeMeso);
        packet.WriteInt(user.Context.Options.CommissionRate);
        packet.WriteInt(user.Context.Options.CommissionBase);
        packet.WriteInt(user.Context.Options.AuctionDurationMin);
        packet.WriteInt(user.Context.Options.AuctionDurationMax);

        packet.WriteDateTime(DateTime.UtcNow);

        await user.Dispatch(packet.Build());
        await user.DispatchUpdateCash();
        await base.Enter(user);
    }
}
