namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ISPWCheck : ILoginStageUserContract
{
    string SPW { get; }
}
