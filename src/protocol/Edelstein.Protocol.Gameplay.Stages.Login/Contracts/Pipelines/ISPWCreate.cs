namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ISPWCreate : ILoginStageUserContract
{
    string SPW { get; }
}
