using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldObjectController : IAdapter, IFieldObject
{
    ICollection<IFieldObjectControllable> Controlled { get; }
}
