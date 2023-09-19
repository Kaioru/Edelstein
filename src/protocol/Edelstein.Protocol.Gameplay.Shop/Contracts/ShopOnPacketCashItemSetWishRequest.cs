namespace Edelstein.Protocol.Gameplay.Shop.Contracts;

public record ShopOnPacketCashItemSetWishRequest(
    IShopStageUser User,
    int[] Wishlist
);
