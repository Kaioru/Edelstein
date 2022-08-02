namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContext
{
    IGameContextOptions Options { get; }
    IGameContextPipelines Pipelines { get; }
    IGameContextTemplates Templates { get; }
}
