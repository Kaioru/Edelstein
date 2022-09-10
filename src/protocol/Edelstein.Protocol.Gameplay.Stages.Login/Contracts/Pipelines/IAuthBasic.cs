namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface IAuthBasic
{
    string Username { get; }
    string Password { get; }
}
