using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Shop.Types;

namespace Edelstein.Protocol.Gameplay.Shop.Contracts;

public record ShopOnPacketCashItemIncSlotCountRequest(
    IShopStageUser User,
    ShopCashType Cash,
    ItemInventoryType Type
);
