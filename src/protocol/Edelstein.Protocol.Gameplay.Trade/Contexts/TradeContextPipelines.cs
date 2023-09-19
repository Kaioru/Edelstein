using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextPipelines(
    IPipeline<StageStart> StageStart,
    IPipeline<StageStop> StageStop,
    IPipeline<UserMigrate<ITradeStageUser>> UserMigrate,
    IPipeline<UserOnPacket<ITradeStageUser>> UserOnPacket,
    IPipeline<UserOnException<ITradeStageUser>> UserOnException,
    IPipeline<UserOnDisconnect<ITradeStageUser>> UserOnDisconnect,
    IPipeline<UserOnPacketAliveAck<ITradeStageUser>> UserOnPacketAliveAck
);
