using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Shop.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContextPipelines(
    IPipeline<StageStart> StageStart,
    IPipeline<StageStop> StageStop,
    IPipeline<UserMigrate<IShopStageUser>> UserMigrate,
    IPipeline<UserOnPacket<IShopStageUser>> UserOnPacket,
    IPipeline<UserOnException<IShopStageUser>> UserOnException,
    IPipeline<UserOnDisconnect<IShopStageUser>> UserOnDisconnect,
    IPipeline<UserOnPacketAliveAck<IShopStageUser>> UserOnPacketAliveAck,
    
    IPipeline<ShopOnPacketCashItemBuyRequest> ShopOnPacketCashItemBuyRequest
);
