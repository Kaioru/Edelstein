using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop;

namespace Edelstein.Common.Gameplay.Shop;

public class ShopStage : AbstractStage<IShopStageUser>, IShopStage
{
    public ShopStage(string id) => ID = id;
    
    public override string ID { get; }

    public async Task Enter(IShopStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
        {
            await user.Disconnect();
            return;
        }
        
        var packet = new PacketWriter(PacketSendOperations.SetCashShop);
        
        packet.WriteCharacterData(user.Character);

        packet.WriteBool(true); // CashShopAuthorized
        packet.WriteString(user.Account.Username);
        
        // SetSaleInfo
        packet.WriteInt(0); // NotSaleInfo
        packet.WriteShort(0); // ModifiedData
        packet.WriteBool(false); // v49

        packet.WriteBytes(new byte[1080]);
        packet.WriteShort(0); // Stock
        packet.WriteShort(0); // LimitGoods
        packet.WriteShort(0); // ZeroGoods

        packet.WriteBool(false); // EventOn
        packet.WriteInt(200); // HighestCharacterLevelInThisAccount

        await user.Dispatch(packet.Build());
        await base.Enter(user);
    }
}
