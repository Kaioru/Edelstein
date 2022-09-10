namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface IAuthLoginToken : ILoginStageUserContract
{
    string Token { get; }
}
