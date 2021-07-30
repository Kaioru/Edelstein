using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IGameStageUser<TStage, TUser> : IMigrateableStageUser<TStage, TUser>
        where TStage : IGameStage<TStage, TUser>
        where TUser : IGameStageUser<TStage, TUser>
    {
        IField Field { get; }
        IFieldObjUser FieldUser { get; }
    }
}
