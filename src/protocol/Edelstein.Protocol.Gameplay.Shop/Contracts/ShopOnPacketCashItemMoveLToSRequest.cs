namespace Edelstein.Protocol.Gameplay.Shop.Contracts;

public record ShopOnPacketCashItemMoveLToSRequest(
    IShopStageUser User,
    long CashItemSN
);
