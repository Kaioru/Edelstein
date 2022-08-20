using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextPipelines : IStageContextPipelines<ILoginStageUser>
{
    IPipeline<ICheckPassword> CheckPassword { get; }
    IPipeline<ISelectWorld> SelectWorld { get; }
    IPipeline<ICheckUserLimit> CheckUserLimit { get; }
    IPipeline<IWorldRequest> WorldRequest { get; }
    IPipeline<ILogoutWorld> LogoutWorld { get; }
    IPipeline<ICheckDuplicatedID> CheckDuplicatedID { get; }
    IPipeline<ICreateNewCharacter> CreateNewCharacter { get; }
    IPipeline<IDeleteCharacter> DeleteCharacter { get; }
    IPipeline<IEnableSPWRequest> EnableSPWRequest { get; }
    IPipeline<ICheckSPWRequest> CheckSPWRequest { get; }
}
