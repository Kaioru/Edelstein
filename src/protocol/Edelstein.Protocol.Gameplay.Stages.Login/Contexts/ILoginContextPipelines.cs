using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextPipelines : IStageContextPipelines<ILoginStageUser>
{
    IPipeline<IAuthLoginBasic> AuthLoginBasic { get; }
    IPipeline<IAuthLoginToken> AuthLoginToken { get; }
    IPipeline<IWorldList> WorldList { get; }
}
