using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IGameStageUser : IStageUser<IGameStageUser>
{
    GameContext Context { get; }

    IField? Field { get; }
    IFieldUser? FieldUser { get; set; }
}
