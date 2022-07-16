namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContext
{
    ILoginContextOptions Options { get; }
    ILoginContextPipelines Pipelines { get; }
}
