using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Plugs;

public class ShopOnPacketCashItemSetWishRequestPlug : IPipelinePlug<ShopOnPacketCashItemSetWishRequest>
{
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemSetWishRequest message)
    {
        if (message.User.Character == null) return;
        
        message.User.Character.Wishlist.Records.Clear();
        for (var i = 0; i < 10; i++)
            message.User.Character.Wishlist.Records.Add(message.Wishlist[i]);

        var p = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        p.WriteByte((byte)ShopResultOperations.SetWish_Done);
        for (var i = 0; i < 10; i++)
            p.WriteInt(message.User.Character.Wishlist.Records[i]);
        await message.User.Dispatch(p.Build());
    }
}
