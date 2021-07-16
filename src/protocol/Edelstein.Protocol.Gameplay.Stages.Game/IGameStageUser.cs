using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IGameStageUser : IStageUser<IGameStage, IGameStageUser>
    {
        IField Field { get; }
        IFieldObjUser FieldUser { get; }
    }
}
