using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ICheckUserLimit : IStageUserContract<ILoginStageUser>
{
    int WorldID { get; }
}
