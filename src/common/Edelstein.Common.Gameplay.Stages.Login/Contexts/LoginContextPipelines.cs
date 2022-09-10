using Edelstein.Protocol.Gameplay.Stages.Contracts.Pipelines;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
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
    IPipeline<IAuthLoginBasic> AuthLoginBasic,
    IPipeline<IWorldList> WorldList,
    IPipeline<IWorldSelect> WorldSelect,
    IPipeline<IWorldSelectReset> WorldSelectReset,
    IPipeline<ISPWCheck> SPWCheck,
    IPipeline<ISPWCreate> SPWCreate,
    IPipeline<ISPWChange> SPWChange,
    IPipeline<ICharacterSelect> CharacterSelect,
    IPipeline<ICharacterCreate> CharacterCreate,
    IPipeline<ICharacterCreateCheckDuplicatedID> CharacterCreateCheckDuplicatedID,
    IPipeline<ICharacterDelete> CharacterDelete
) : ILoginContextPipelines;
