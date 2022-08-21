using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICheckPassword : IStageUserContract<ILoginStageUser>
{
    string Username { get; }
    string Password { get; }
}
