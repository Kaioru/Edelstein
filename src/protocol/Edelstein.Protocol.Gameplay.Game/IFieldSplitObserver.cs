using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSplitObserver : IAdapter, IFieldObject
{
    ICollection<IFieldSplit> Observing { get; }
}
