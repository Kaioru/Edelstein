using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextPipelines : IStageContextPipelines<ILoginStageUser>
{
    IPipeline<IAuthLoginBasic> AuthLoginBasic { get; }
    IPipeline<IWorldList> WorldList { get; }
    IPipeline<IWorldSelect> WorldSelect { get; }
    IPipeline<IWorldSelectReset> WorldSelectReset { get; }
    IPipeline<ISPWCheck> SPWCheck { get; }
    IPipeline<ISPWCreate> SPWCreate { get; }
    IPipeline<ISPWChange> SPWChange { get; }
    IPipeline<ICharacterSelect> CharacterSelect { get; }
    IPipeline<ICharacterCreate> CharacterCreate { get; }
    IPipeline<ICharacterCheckDuplicatedID> CharacterCheckDuplicatedID { get; }
    IPipeline<ICharacterDelete> CharacterDelete { get; }
}
