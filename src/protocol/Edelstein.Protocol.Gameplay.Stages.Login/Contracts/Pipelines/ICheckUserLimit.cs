using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICheckUserLimit : IStageUserContract<ILoginStageUser>
{
    int WorldID { get; }
}
