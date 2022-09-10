namespace Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

public interface ISPWChange : ILoginStageUserContract
{
    string SPWCurrent { get; }
    string SPWNew { get; }
}
