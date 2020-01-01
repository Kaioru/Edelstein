namespace Edelstein.Service.Game.Fields.Objects.User.Stats
{
    public class ForcedStat
    {
        public short? STR { get; internal set; }
        public short? DEX { get; internal set; }
        public short? INT { get; internal set; }
        public short? LUK { get; internal set; }

        public short? PAD { get; internal set; }
        public short? PDD { get; internal set; }
        public short? MAD { get; internal set; }
        public short? MDD { get; internal set; }

        public short? ACC { get; internal set; }
        public short? EVA { get; internal set; }

        public byte? Speed { get; internal set; }
        public byte? Jump { get; internal set; }
        public byte? SpeedMax { get; internal set; }

        private readonly FieldUser _user;

        public ForcedStat(FieldUser user)
            => _user = user;
    }
}