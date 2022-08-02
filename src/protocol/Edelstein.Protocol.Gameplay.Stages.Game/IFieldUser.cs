using Edelstein.Protocol.Gameplay.Stages.Game.Objects;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldUser : IStageUser<IFieldUser>, IFieldLife, IFieldSplitObserver, IFieldController
{
}
