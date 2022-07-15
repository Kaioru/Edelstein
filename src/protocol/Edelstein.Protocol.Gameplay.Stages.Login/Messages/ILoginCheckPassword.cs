namespace Edelstein.Protocol.Gameplay.Stages.Login.Messages;

public interface ILoginCheckPassword : ILoginMessage
{
    string Username { get; }
    string Password { get; }
}
