using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats
{
    public class ForcedStats : IForcedStats
    {
        public short? STR { get; set; }
        public short? DEX { get; set; }
        public short? INT { get; set; }
        public short? LUK { get; set; }
        public short? PAD { get; set; }
        public short? PDD { get; set; }
        public short? MAD { get; set; }
        public short? MDD { get; set; }
        public short? ACC { get; set; }
        public short? EVA { get; set; }
        public byte? Speed { get; set; }
        public byte? Jump { get; set; }
        public byte? SpeedMax { get; set; }
    }
}
