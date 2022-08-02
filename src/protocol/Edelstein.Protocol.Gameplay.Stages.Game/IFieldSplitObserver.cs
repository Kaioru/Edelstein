using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldSplitObserver : IAdapter
{
    ICollection<IFieldSplit> Observing { get; }
}
