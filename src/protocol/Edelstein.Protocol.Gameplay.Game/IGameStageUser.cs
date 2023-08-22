using Edelstein.Protocol.Gameplay.Game.Contexts;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IGameStageUser : IStageUser<IGameStageUser>
{
    GameContext Context { get; }
}
