namespace Edelstein.Provider.Templates.Field.Continent
{
    public class ContinentTemplate : ITemplate
    {
        public int ID { get; }

        public string Info { get; set; }

        public int StartShipMoveFieldID { get; set; }
        public int WaitFieldID { get; set; }
        public int MoveFieldID { get; set; }
        public int? CabinFieldID { get; set; }
        public int EndFieldID { get; set; }
        public int EndShipMoveFieldID { get; set; }

        public int Term { get; set; }
        public int Delay { get; set; }

        public bool Event { get; set; }
        public ContinentGenMobTemplate? GenMob { get; set; }

        public int Wait { get; set; }
        public int EventEnd { get; set; }
        public int Required { get; set; }

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
                GenMob = new ContinentGenMobTemplate(0, g)
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