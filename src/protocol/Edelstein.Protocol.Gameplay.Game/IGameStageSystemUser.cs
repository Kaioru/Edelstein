using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IGameStageSystemUser : IStageSystemUser<IGameStageSystem, IGameStageSystemUser>
{
    IFieldUser? FieldUser { get; set; }
}
