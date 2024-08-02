using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<SystemOnStart> SystemOnStart,
    IPipeline<SystemOnStop> SystemOnStop,

    IPipeline<UserOnPacket<ILoginStageSystemUser, ILoginStageSystem>> UserOnPacket,
    IPipeline<UserOnException<ILoginStageSystemUser, ILoginStageSystem>> UserOnException,
    IPipeline<UserOnDisconnect<ILoginStageSystemUser, ILoginStageSystem>> UserOnDisconnect,

    IPipeline<PipedPacketMessage<ILoginStageSystemUser, CheckPassword>> UserOnPacketCheckPassword 
);
