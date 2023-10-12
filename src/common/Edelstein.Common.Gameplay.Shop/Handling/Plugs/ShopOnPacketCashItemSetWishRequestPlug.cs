using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Shop.Types;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Shop.Handling.Plugs;

public class ShopOnPacketCashItemSetWishRequestPlug : IPipelinePlug<ShopOnPacketCashItemSetWishRequest>
{
    public async Task Handle(IPipelineContext ctx, ShopOnPacketCashItemSetWishRequest message)
    {
        if (message.User.Character == null) return;
        
        message.User.Character.Wishlist.Records.Clear();
        for (var i = 0; i < 10; i++)
            message.User.Character.Wishlist.Records.Add(message.Wishlist[i]);

        using var packet = new PacketWriter(PacketSendOperations.CashShopCashItemResult);

        packet.WriteByte((byte)ShopResultOperations.SetWish_Done);
        for (var i = 0; i < 10; i++)
            packet.WriteInt(message.User.Character.Wishlist.Records[i]);
        await message.User.Dispatch(packet.Build());
    }
}
