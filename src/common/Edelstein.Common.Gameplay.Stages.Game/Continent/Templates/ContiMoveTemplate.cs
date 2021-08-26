using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Continent.Templates
{
    public record ContiMoveTemplate : ITemplate
    {
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

        public bool Event { get; }
        public int? GenMobItemID { get; }
        public Point2D? GenMobPosition { get; }

        public int Wait { get; }
        public int EventEnd { get; }
        public int Required { get; }

        public ContiMoveTemplate(
            int id,
            IDataProperty property,
            IDataProperty field,
            IDataProperty scheduler,
            IDataProperty genMob,
            IDataProperty time
        )
        {
            ID = id;

            Name = property.ResolveOrDefault<string>("info");

            StartShipMoveFieldID = field.Resolve<int>("startShipMoveFieldID") ?? 999999999;
            WaitFieldID = field.Resolve<int>("waitFieldID") ?? 999999999;
            MoveFieldID = field.Resolve<int>("moveFieldID") ?? 999999999;
            CabinFieldID = field.Resolve<int>("cabinFieldID");
            EndFieldID = field.Resolve<int>("endFieldID") ?? 999999999;
            EndShipMoveFieldID = field.Resolve<int>("endShipMoveFieldID") ?? 999999999;

            Term = scheduler.Resolve<int>("tTerm") ?? 1;
            Delay = scheduler.Resolve<int>("tDelay") ?? 0;

            Event = genMob != null;
            GenMobItemID = genMob?.Resolve<int>("genMobItemID") ?? 0;
            GenMobPosition = Event ? new Point2D(
                 genMob?.Resolve<int>("genMobPosition_x") ?? 0,
                 genMob?.Resolve<int>("genMobPosition_y") ?? 0
             ) : null;

            Wait = time.Resolve<int>("tWait") ?? 1;
            EventEnd = time.Resolve<int>("tEventEnd") ?? 0;
            Required = time.Resolve<int>("tRequired") ?? 0;
        }
    }
}
