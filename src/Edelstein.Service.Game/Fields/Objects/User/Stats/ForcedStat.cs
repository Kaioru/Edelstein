using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Stats
{
    public class ForcedStat
    {
        private readonly FieldUser _user;

        public short STR { get; set; }
        public short DEX { get; set; }
        public short INT { get; set; }
        public short LUK { get; set; }
        public short PAD { get; set; }
        public short PDD { get; set; }
        public short MAD { get; set; }
        public short MDD { get; set; }
        public short ACC { get; set; }
        public short EVA { get; set; }
        public byte Speed { get; set; }
        public byte Jump { get; set; }
        public byte SpeedMax { get; set; }

        public ForcedStat(FieldUser user)
        {
            _user = user;
        }

        public void Reset()
        {
            STR = 0;
            DEX = 0;
            INT = 0;
            LUK = 0;
            PAD = 0;
            PDD = 0;
            MAD = 0;
            MDD = 0;
            ACC = 0;
            EVA = 0;
            Speed = 0;
            Jump = 0;
            SpeedMax = 0;

            _user.SendPacket(new Packet(SendPacketOperations.ForcedStatReset));
        }
    }
}