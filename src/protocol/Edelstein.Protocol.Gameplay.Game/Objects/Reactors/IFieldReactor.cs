using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Reactors;

public interface IFieldReactor : IFieldObject
{
    IReactorTemplate Template { get; }
}
