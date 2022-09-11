namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface IAuthLoginBasic : ILoginStageUserContract
{
    string Username { get; }
    string Password { get; }
}
