using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextPipelines(
    IPipeline<SystemOnStart> SystemOnStart,
    IPipeline<SystemOnStop> SystemOnStop,

    IPipeline<UserOnPacket<IGameStageSystem, IGameStageSystemUser>> UserOnPacket,
    IPipeline<UserOnException<IGameStageSystem, IGameStageSystemUser>> UserOnException,
    IPipeline<UserOnDisconnect<IGameStageSystem, IGameStageSystemUser>> UserOnDisconnect,
    
    IPipeline<PipedPacketMessage<IGameStageSystem, IGameStageSystemUser, MigrateIn>> UserOnPacketMigrateIn 
);
