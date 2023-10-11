using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;

namespace Edelstein.Common.Gameplay.Game.Continents.Templates;

public record ContiMoveTemplate : IContiMoveTemplate
{
    public ContiMoveTemplate(
        int id,
        IDataNode node,
        IDataNode field,
        IDataNode scheduler,
        IDataNode? genMob,
        IDataNode time
    )
    {
        ID = id;

        Name = node.ResolveString("info") ?? "NO-NAME";

        StartShipMoveFieldID = field.ResolveInt("startShipMoveFieldID") ?? 999999999;
        WaitFieldID = field.ResolveInt("waitFieldID") ?? 999999999;
        MoveFieldID = field.ResolveInt("moveFieldID") ?? 999999999;
        CabinFieldID = field.ResolveInt("cabinFieldID");
        EndFieldID = field.ResolveInt("endFieldID") ?? 999999999;
        EndShipMoveFieldID = field.ResolveInt("endShipMoveFieldID") ?? 999999999;

        Term = scheduler.ResolveInt("tTerm") ?? 1;
        Delay = scheduler.ResolveInt("tDelay") ?? 0;

        if (genMob != null) GenMob = new ContiMoveTemplateGenMob(genMob);

        Wait = time.ResolveInt("tWait") ?? 1;
        EventEnd = time.ResolveInt("tEventEnd") ?? 0;
        Required = time.ResolveInt("tRequired") ?? 0;
    }

    public int ID { get; }

    public string Name { get; }

    public int StartShipMoveFieldID { get; }
    public int WaitFieldID { get; }
    public int MoveFieldID { get; }
    public int? CabinFieldID { get; }
    public int EndFieldID { get; }
    public int EndShipMoveFieldID { get; }

    public int Term { get; }
    public int Delay { get; }

    public bool Event => GenMob != null;
    public IContiMoveTemplateGenMob? GenMob { get; }

    public int Wait { get; }
    public int EventEnd { get; }
    public int Required { get; }
}
