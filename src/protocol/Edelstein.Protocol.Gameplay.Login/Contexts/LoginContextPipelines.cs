using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<ServerStartLoad> ServerStartLoad,
    IPipeline<ServerStartInit> ServerStartInit,
    IPipeline<ServerStart> ServerStart,
    IPipeline<ServerStop> ServerStop,
    
    IPipeline<UserMigrate<ILoginStageUser>> UserMigrate,
    IPipeline<UserOnPacket<ILoginStageUser>> UserOnPacket,
    IPipeline<UserOnException<ILoginStageUser>> UserOnException,
    IPipeline<UserOnDisconnect<ILoginStageUser>> UserOnDisconnect,
    
    IPipeline<UserOnPacketMigrateIn<ILoginStageUser>> UserOnPacketMigrateIn,
    IPipeline<UserOnPacketAliveAck<ILoginStageUser>> UserOnPacketAliveAck,
    IPipeline<OnUserPacketCheckPassword> OnUserPacketCheckPassword,
    IPipeline<OnUserPacketSelectWorld> OnUserPacketSelectWorld,
    IPipeline<OnUserPacketCheckUserLimit> OnUserPacketCheckUserLimit,
    IPipeline<OnUserPacketWorldRequest> OnUserPacketWorldRequest,
    IPipeline<OnUserPacketLogoutWorld> OnUserPacketLogoutWorld,
    IPipeline<OnUserPacketCheckDuplicatedID> OnUserPacketCheckDuplicatedID,
    IPipeline<OnUserPacketCreateNewCharacter> OnUserPacketCreateNewCharacter,
    IPipeline<OnUserPacketDeleteCharacter> OnUserPacketDeleteCharacter,
    IPipeline<OnUserPacketEnableSPWRequest> OnUserPacketEnableSPWRequest,
    IPipeline<OnUserPacketCheckSPWRequest> OnUserPacketCheckSPWRequest
);
