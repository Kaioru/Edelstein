using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continents.Templates;

public interface IContiMoveTemplate : ITemplate
{
    string Name { get; }

    int Term { get; }
    int Delay { get; }

    bool Event { get; }

    int Wait { get; }
    int EventEnd { get; }
    int Required { get; }
}
