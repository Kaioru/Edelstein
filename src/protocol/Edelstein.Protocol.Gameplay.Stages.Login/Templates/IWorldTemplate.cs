using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Templates;

public interface IWorldTemplate : ITemplate
{
    string Name { get; }
    byte State { get; }
    bool BlockCharCreation { get; }
}
