using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

namespace Edelstein.Common.Gameplay.Stages.Game.Contexts;

public record GameContext(
    IGameContextOptions Options,
    IGameContextPipelines Pipelines,
    IGameContextTemplates Templates
) : IGameContext;
