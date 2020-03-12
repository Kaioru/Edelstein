using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Continent
{
    public class ContinentTemplate : IDataTemplate
    {
        public int ID { get; }

        public string Info { get; }

        public int StartShipMoveFieldID { get; private set; }
        public int WaitFieldID { get; private set; }
        public int MoveFieldID { get; private set; }
        public int? CabinFieldID { get; private set; }
        public int EndFieldID { get; private set; }
        public int EndShipMoveFieldID { get; private set; }

        public int Term { get; private set; }
        public int Delay { get; private set; }

        public bool Event { get; }
        public ContinentGenMobTemplate GenMob { get; private set; }

        public int Wait { get; private set; }
        public int EventEnd { get; private set; }
        public int Required { get; private set; }

        public ContinentTemplate(int id, IDataProperty property)
        {
            ID = id;

            Info = property.ResolveOrDefault<string>("info");

            property.Resolve("field").ResolveAll(f =>
            {
                StartShipMoveFieldID = f.Resolve<int>("startShipMoveFieldID") ?? 999999999;
                WaitFieldID = f.Resolve<int>("waitFieldID") ?? 999999999;
                MoveFieldID = f.Resolve<int>("moveFieldID") ?? 999999999;
                CabinFieldID = f.Resolve<int>("cabinFieldID");
                EndFieldID = f.Resolve<int>("endFieldID") ?? 999999999;
                EndShipMoveFieldID = f.Resolve<int>("endShipMoveFieldID") ?? 999999999;
            });

            property.Resolve("scheduler").ResolveAll(s =>
            {
                Term = s.Resolve<int>("tTerm") ?? 1;
                Delay = s.Resolve<int>("tDelay") ?? 0;
            });

            property.Resolve("genMob")?.ResolveAll(g =>
                GenMob = new ContinentGenMobTemplate(g.ResolveAll())
            );

            Event = GenMob != null;

            property.Resolve("time").ResolveAll(t =>
            {
                Wait = t.Resolve<int>("tWait") ?? 1;
                EventEnd = t.Resolve<int>("tEventEnd") ?? 0;
                Required = t.Resolve<int>("tRequired") ?? 0;
            });
        }
    }
}