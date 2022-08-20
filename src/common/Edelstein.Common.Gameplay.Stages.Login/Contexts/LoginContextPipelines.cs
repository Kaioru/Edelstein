using Edelstein.Protocol.Gameplay.Stages.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contexts;

public record LoginContextPipelines(
    IPipeline<ISocketOnMigrateIn<ILoginStageUser>> SocketOnMigrateIn,
    IPipeline<ISocketOnMigrateOut<ILoginStageUser>> SocketOnMigrateOut,
    IPipeline<ISocketOnAliveAck<ILoginStageUser>> SocketOnAliveAck,
    IPipeline<ISocketOnPacket<ILoginStageUser>> SocketOnPacket,
    IPipeline<ISocketOnException<ILoginStageUser>> SocketOnException,
    IPipeline<ISocketOnDisconnect<ILoginStageUser>> SocketOnDisconnect,
    IPipeline<ICheckPassword> CheckPassword,
    IPipeline<ISelectWorld> SelectWorld,
    IPipeline<ICheckUserLimit> CheckUserLimit,
    IPipeline<IWorldRequest> WorldRequest,
    IPipeline<ILogoutWorld> LogoutWorld,
    IPipeline<ICheckDuplicatedID> CheckDuplicatedID,
    IPipeline<ICreateNewCharacter> CreateNewCharacter,
    IPipeline<IDeleteCharacter> DeleteCharacter,
    IPipeline<IEnableSPWRequest> EnableSPWRequest,
    IPipeline<ICheckSPWRequest> CheckSPWRequest
) : ILoginContextPipelines;
