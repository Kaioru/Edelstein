namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContext
{
    ILoginContextOptions Options { get; }
    ILoginContextEvents Events { get; }
    ILoginContextPipelines Pipelines { get; }
    ILoginContextServices Services { get; }
    ILoginContextManagers Managers { get; }
    ILoginContextTemplates Templates { get; }
}
