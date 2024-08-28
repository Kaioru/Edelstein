using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<SystemOnStart> SystemOnStart,
    IPipeline<SystemOnStop> SystemOnStop,

    IPipeline<UserOnPacket<ILoginStageSystem, ILoginStageSystemUser>> UserOnPacket,
    IPipeline<UserOnException<ILoginStageSystem, ILoginStageSystemUser>> UserOnException,
    IPipeline<UserOnDisconnect<ILoginStageSystem, ILoginStageSystemUser>> UserOnDisconnect,

    IPipeline<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, CheckPassword>> UserOnPacketCheckPassword ,
    IPipeline<PipedPacketMessage<ILoginStageSystem, ILoginStageSystemUser, WorldRequest>> UserOnPacketWorldRequest 
);
