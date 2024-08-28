using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextPipelines(
    IPipeline<SystemOnStart> SystemOnStart,
    IPipeline<SystemOnStop> SystemOnStop,

    IPipeline<UserOnPacket<IGameStageSystem, IGameStageSystemUser>> UserOnPacket,
    IPipeline<UserOnException<IGameStageSystem, IGameStageSystemUser>> UserOnException,
    IPipeline<UserOnDisconnect<IGameStageSystem, IGameStageSystemUser>> UserOnDisconnect
);
