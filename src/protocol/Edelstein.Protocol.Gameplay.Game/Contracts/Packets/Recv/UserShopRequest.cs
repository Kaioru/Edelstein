using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserShopRequest : StructuredRecvPacket
{
    [FieldOrder(0)] public required ShopDialogRequestType Type { get; init; }
    
    [FieldOrder(1)]
    [Subtype(nameof(Type), ShopDialogRequestType.Buy, typeof(UserShopRequestInfoBuy))]
    [Subtype(nameof(Type), ShopDialogRequestType.Sell, typeof(UserShopRequestInfoSell))]
    [Subtype(nameof(Type), ShopDialogRequestType.Recharge, typeof(UserShopRequestInfoRecharge))]
    [Subtype(nameof(Type), ShopDialogRequestType.Close, typeof(UserShopRequestInfoClose))]
    [SubtypeDefault(typeof(UserShopRequestInfo))]
    public required UserShopRequestInfo Info { get; init; }
}

public record UserShopRequestInfo : StructuredBasePacket;

public record UserShopRequestInfoBuy : UserShopRequestInfo
{
    [FieldOrder(0)] public short BuySelected { get; init; }
    [FieldOrder(1)] public int ItemID { get; init; }
    [FieldOrder(2)] public short Count { get; init; }
    [FieldOrder(3)] public int DiscountPrice { get; init; }
}

public record UserShopRequestInfoSell : UserShopRequestInfo
{
    [FieldOrder(0)] public short POS { get; init; }
    [FieldOrder(1)] public int ItemID { get; init; }
    [FieldOrder(2)] public short Count { get; init; }
}

public record UserShopRequestInfoRecharge : UserShopRequestInfo
{
    [FieldOrder(0)] public short POS { get; init; }
}

public record UserShopRequestInfoClose : UserShopRequestInfo;
