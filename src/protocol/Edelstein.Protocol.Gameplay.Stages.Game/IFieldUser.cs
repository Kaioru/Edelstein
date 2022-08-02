using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldUser : IFieldObject, IStageUser<IFieldUser>
{
    bool IsInstantiated { get; }
}
