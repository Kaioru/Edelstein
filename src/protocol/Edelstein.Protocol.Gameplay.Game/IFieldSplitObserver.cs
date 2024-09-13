using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Game;

public interface IFieldSplitObserver : IFieldObject, ISocketUser
{
    ICollection<IFieldSplit> Observing { get; }
}
