namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContext
{
    ILoginContextPipelines Pipelines { get; }
}
