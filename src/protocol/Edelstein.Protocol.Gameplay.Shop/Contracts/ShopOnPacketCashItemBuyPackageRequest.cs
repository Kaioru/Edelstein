using Edelstein.Protocol.Gameplay.Shop.Types;

namespace Edelstein.Protocol.Gameplay.Shop.Contracts;

public record ShopOnPacketCashItemBuyPackageRequest(
    IShopStageUser User,
    ShopCashType Cash,
    int CommoditySN
);
