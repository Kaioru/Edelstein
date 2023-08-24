using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextPipelines(
    IPipeline<StageStart> StageStart,
    IPipeline<StageStop> StageStop,
    IPipeline<UserMigrate<IGameStageUser>> UserMigrate,
    IPipeline<UserOnPacket<IGameStageUser>> UserOnPacket,
    IPipeline<UserOnException<IGameStageUser>> UserOnException,
    IPipeline<UserOnDisconnect<IGameStageUser>> UserOnDisconnect,
    
    IPipeline<UserOnPacketAliveAck<IGameStageUser>> UserOnPacketAliveAck,
    IPipeline<UserOnPacketMigrateIn<IGameStageUser>> UserOnPacketMigrateIn,
    
    IPipeline<FieldOnPacketUserMove> FieldOnPacketUserMove,
    IPipeline<FieldOnPacketUserChat> FieldOnPacketUserChat,
    IPipeline<FieldOnPacketUserEmotion> FieldOnPacketUserEmotion,
    IPipeline<FieldOnPacketUserSelectNPC> FieldOnPacketUserSelectNPC,
    IPipeline<FieldOnPacketUserGatherItemRequest> FieldOnPacketUserGatherItemRequest,
    IPipeline<FieldOnPacketUserSortItemRequest> FieldOnPacketUserSortItemRequest,
    IPipeline<FieldOnPacketUserChangeSlotPositionRequest> FieldOnPacketUserChangeSlotPositionRequest,
    
    IPipeline<FieldOnPacketNPCMove> FieldOnPacketNPCMove,
    
    IPipeline<FieldOnPacketDropPickupRequest> FieldOnPacketDropPickupRequest
);
