using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects;

public interface IFieldController : IAdapter, IFieldObject
{
    ICollection<IFieldControllable> Controlled { get; }
}
