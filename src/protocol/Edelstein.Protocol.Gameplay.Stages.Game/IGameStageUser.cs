using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IGameStageUser : IStageUser<IGameStageUser>
{
    IGameContext Context { get; }
}
