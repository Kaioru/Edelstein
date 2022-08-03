using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldSplitObserver : IAdapter, IFieldObject
{
    ICollection<IFieldSplit> Observing { get; }
}
