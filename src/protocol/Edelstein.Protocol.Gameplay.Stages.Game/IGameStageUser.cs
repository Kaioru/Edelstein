using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IGameStageUser : IStageUser<IGameStageUser>
{
    IGameContext Context { get; }

    IField? Field { get; }
    IFieldUser? FieldUser { get; set; }
}
