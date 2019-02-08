using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.Continent
{
    public class ContinentTemplate : ITemplate
    {
        public int ID { get; set; }

        public string Info { get; set; }

        public int StartShipMoveFieldID { get; set; }
        public int WaitFieldID { get; set; }
        public int MoveFieldID { get; set; }
        public int? CabinFieldID { get; set; }
        public int EndFieldID { get; set; }
        public int EndShipMoveFieldID { get; set; }

        public int Term { get; set; }
        public int Delay { get; set; }
        
        public int Wait { get; set; }
        public int EventEnd { get; set; }
        public int Required { get; set; }

        public static ContinentTemplate Parse(int id, IDataProperty property)
        {
            var t = new ContinentTemplate {ID = id};

            property.Resolve(p =>
            {
                t.Info = p.ResolveOrDefault<string>("info");

                p.Resolve("field").Resolve(f =>
                {
                    t.StartShipMoveFieldID = f.Resolve<int>("startShipMoveFieldID") ?? 999999999;
                    t.WaitFieldID = f.Resolve<int>("waitFieldID") ?? 999999999;
                    t.MoveFieldID = f.Resolve<int>("moveFieldID") ?? 999999999;
                    t.CabinFieldID = f.Resolve<int>("cabinFieldID");
                    t.EndFieldID = f.Resolve<int>("endFieldID") ?? 999999999;
                    t.EndShipMoveFieldID = f.Resolve<int>("endShipMoveFieldID") ?? 999999999;
                });
                
                p.Resolve("scheduler").Resolve(s =>
                {
                    t.Term = s.Resolve<int>("tTerm") ?? 1;
                    t.Delay = s.Resolve<int>("tDelay") ?? 0;
                });
                
                p.Resolve("time").Resolve(d =>
                {
                    t.Wait = d.Resolve<int>("tWait") ?? 1;
                    t.EventEnd = d.Resolve<int>("tEventEnd") ?? 0;
                    t.Required = d.Resolve<int>("tRequired") ?? 0;
                });
            });

            return t;
        }
    }
}