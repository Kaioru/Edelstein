namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ICheckLoginAuthInfo : ILoginStageUserContract
{
    string Username { get; }
    string Password { get; }
}
