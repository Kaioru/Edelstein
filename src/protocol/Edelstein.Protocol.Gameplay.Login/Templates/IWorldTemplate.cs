using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Login.Templates;

public interface IWorldTemplate : ITemplate
{
    string Name { get; }
    byte State { get; }
    bool BlockCharCreation { get; }
}
