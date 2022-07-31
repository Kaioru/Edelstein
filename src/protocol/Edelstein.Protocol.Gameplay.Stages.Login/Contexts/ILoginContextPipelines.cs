using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextPipelines : IStageContextPipelines<ILoginStageUser>
{
    IPipeline<ICheckPassword> CheckPassword { get; }
    IPipeline<ICheckUserLimit> CheckUserLimit { get; }
    IPipeline<IWorldRequest> WorldRequest { get; }
}
