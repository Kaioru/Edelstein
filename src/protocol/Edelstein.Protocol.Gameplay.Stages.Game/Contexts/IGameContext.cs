﻿namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContext
{
    IGameContextOptions Options { get; }
    IGameContextPipelines Pipelines { get; }
    IGameContextServices Services { get; }
    IGameContextManagers Managers { get; }
    IGameContextTemplates Templates { get; }
}
