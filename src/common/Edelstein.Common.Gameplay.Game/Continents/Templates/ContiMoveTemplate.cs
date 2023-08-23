using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;

namespace Edelstein.Common.Gameplay.Game.Continents.Templates;

public record ContiMoveTemplate : IContiMoveTemplate
{
    public ContiMoveTemplate(
        int id,
        IDataProperty property,
        IDataProperty field,
        IDataProperty scheduler,
        IDataProperty? genMob,
        IDataProperty time
    )
    {
        ID = id;

        Name = property.ResolveOrDefault<string>("info") ?? "NO-NAME";

        StartShipMoveFieldID = field.Resolve<int>("startShipMoveFieldID") ?? 999999999;
        WaitFieldID = field.Resolve<int>("waitFieldID") ?? 999999999;
        MoveFieldID = field.Resolve<int>("moveFieldID") ?? 999999999;
        CabinFieldID = field.Resolve<int>("cabinFieldID");
        EndFieldID = field.Resolve<int>("endFieldID") ?? 999999999;
        EndShipMoveFieldID = field.Resolve<int>("endShipMoveFieldID") ?? 999999999;

        Term = scheduler.Resolve<int>("tTerm") ?? 1;
        Delay = scheduler.Resolve<int>("tDelay") ?? 0;

        if (genMob != null) GenMob = new ContiMoveTemplateGenMob(genMob);

        Wait = time.Resolve<int>("tWait") ?? 1;
        EventEnd = time.Resolve<int>("tEventEnd") ?? 0;
        Required = time.Resolve<int>("tRequired") ?? 0;
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
