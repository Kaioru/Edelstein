using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContext(
    IGameContextOptions Options,
    IGameContextEvents Events,
    IGameContextPipelines Pipelines,
    IGameContextServices Services,
    IGameContextManagers Managers,
    IGameContextTemplates Templates
) : IGameContext;
