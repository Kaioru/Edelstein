using Edelstein.Protocol.Gameplay.Shop.Types;

namespace Edelstein.Protocol.Gameplay.Shop.Contracts;

public record ShopOnPacketCashItemBuyRequest(
    IShopStageUser User,
    ShopCashType Cash,
    int CommoditySN
);
