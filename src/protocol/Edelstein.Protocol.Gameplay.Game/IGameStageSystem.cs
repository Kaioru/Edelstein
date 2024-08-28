using Edelstein.Protocol.Gameplay.Game.Contexts;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IGameStageSystem : IStageSystem<IGameStageSystem, IGameStageSystemUser>
{
    IGameStageSystemOptions Options { get; }
    GameContext Context { get; }
}
