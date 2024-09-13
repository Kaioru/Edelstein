using System.Collections.Generic;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectController : ISocketUser, IFieldObject
{
    ICollection<IFieldObjectControllable> Controlling { get; }
}
