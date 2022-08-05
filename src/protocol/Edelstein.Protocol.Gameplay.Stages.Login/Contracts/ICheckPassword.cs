using Edelstein.Protocol.Gameplay.Stages.Contracts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

public interface ICheckPassword : IStageUserMessage<ILoginStageUser>
{
    string Username { get; }
    string Password { get; }
}
