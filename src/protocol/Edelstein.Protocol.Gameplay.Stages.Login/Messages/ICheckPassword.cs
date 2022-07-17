using Edelstein.Protocol.Gameplay.Stages.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface ICheckPassword : IStageUserMessage<ILoginStageUser>
{
    string Username { get; }
    string Password { get; }
}
