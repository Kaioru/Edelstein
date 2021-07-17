using Edelstein.Protocol.Gameplay.Templating;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent.Templates
{
    public record ContiMoveTemplate : ITemplate
    {
        public int ID { get; init; }

        public string Info { get; init; }

        public int StartShipMoveFieldID { get; init; }
        public int WaitFieldID { get; init; }
        public int MoveFieldID { get; init; }
        public int? CabinFieldID { get; init; }
        public int EndFieldID { get; init; }
        public int EndShipMoveFieldID { get; init; }

        public int Term { get; init; }
        public int Delay { get; init; }

        public bool Event { get; }
        public ContiMoveGenMobTemplate GenMob { get; init; }

        public int Wait { get; init; }
        public int EventEnd { get; init; }
        public int Required { get; init; }
    }
}
