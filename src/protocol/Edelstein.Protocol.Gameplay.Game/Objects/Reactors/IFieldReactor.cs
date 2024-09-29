using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Reactors;

public interface IFieldReactor : IFieldObject
{
    IReactorTemplate Template { get; }
    string? Name { get; }
    byte State { get; }

    Task SetState(byte state, short delay = 0, byte properEventIdx = 0, byte stateEnd = 0);
}
