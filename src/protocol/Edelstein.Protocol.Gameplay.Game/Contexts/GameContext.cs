namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContext(
    IGameStage Stage,
    IGameStageOptions Options,
    GameContextManagers Managers,
    GameContextServices Services,
    GameContextRepositories Repositories,
    GameContextTemplates Templates,
    GameContextPipelines Pipelines
);
