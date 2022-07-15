namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface ICheckPassword : ILoginMessage
{
    string Username { get; }
    string Password { get; }
}
