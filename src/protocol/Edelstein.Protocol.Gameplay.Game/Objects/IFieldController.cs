using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldController : IAdapter, IFieldObject
{
    ICollection<IFieldControllable> Controlled { get; }
}
