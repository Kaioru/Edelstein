using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Continents.Templates;

public interface IContiMoveTemplate : ITemplate
{
    string Name { get; }

    int StartShipMoveFieldID { get; }
    int WaitFieldID { get; }
    int MoveFieldID { get; }
    int? CabinFieldID { get; }
    int EndFieldID { get; }
    int EndShipMoveFieldID { get; }

    int Term { get; }
    int Delay { get; }

    bool Event { get; }
    IContiMoveTemplateGenMob? GenMob { get; }

    int Wait { get; }
    int EventEnd { get; }
    int Required { get; }
}
