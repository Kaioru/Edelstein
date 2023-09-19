using Edelstein.Protocol.Gameplay.Models.Inventories;

namespace Edelstein.Protocol.Gameplay.Shop.Contracts;

public record ShopOnPacketCashItemMoveSToLRequest(
    IShopStageUser User,
    long CashItemSN,
    ItemInventoryType Type
);
