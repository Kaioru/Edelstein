using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<StageStart> StageStart,
    IPipeline<StageStop> StageStop,
    IPipeline<UserMigrate<ILoginStageUser>> UserMigrate,
    IPipeline<UserOnPacket<ILoginStageUser>> UserOnPacket,
    IPipeline<UserOnException<ILoginStageUser>> UserOnException,
    IPipeline<UserOnDisconnect<ILoginStageUser>> UserOnDisconnect,
    IPipeline<UserOnPacketAliveAck<ILoginStageUser>> UserOnPacketAliveAck,
    
    IPipeline<UserOnPacketCheckPassword> UserOnPacketCheckPassword,
    IPipeline<UserOnPacketWorldRequest> UserOnPacketWorldRequest,
    IPipeline<UserOnPacketSelectWorld> UserOnPacketSelectWorld,
    IPipeline<UserOnPacketCheckUserLimit> UserOnPacketCheckUserLimit,
    IPipeline<UserOnPacketLogoutWorld> UserOnPacketLogoutWorld,
    IPipeline<UserOnPacketCheckDuplicatedID> UserOnPacketCheckDuplicatedID,
    IPipeline<UserOnPacketCreateNewCharacter> UserOnPacketCreateNewCharacter,
    IPipeline<UserOnPacketDeleteCharacter> UserOnPacketDeleteCharacter,
    IPipeline<UserOnPacketEnableSPWRequest> UserOnPacketEnableSPWRequest,
    IPipeline<UserOnPacketCheckSPWRequest> UserOnPacketCheckSPWRequest,
    IPipeline<UserOnPacketCreateSecurityHandle> UserOnPacketCreateSecurityHandle
);
