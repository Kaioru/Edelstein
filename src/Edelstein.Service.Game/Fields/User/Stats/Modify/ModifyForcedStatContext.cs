using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Fields.User.Stats.Modify
{
    public class ModifyForcedStatContext
    {
        private readonly ForcedStat _forcedStat;
        private ModifyForcedStatType _flag = 0;

        public short STR
        {
            get => _forcedStat.STR;
            set
            {
                _flag |= ModifyForcedStatType.STR;
                _forcedStat.STR = value;
            }
        }

        public short DEX
        {
            get => _forcedStat.DEX;
            set
            {
                _flag |= ModifyForcedStatType.DEX;
                _forcedStat.DEX = value;
            }
        }

        public short INT
        {
            get => _forcedStat.INT;
            set
            {
                _flag |= ModifyForcedStatType.INT;
                _forcedStat.INT = value;
            }
        }

        public short LUK
        {
            get => _forcedStat.LUK;
            set
            {
                _flag |= ModifyForcedStatType.LUK;
                _forcedStat.LUK = value;
            }
        }

        public short PAD
        {
            get => _forcedStat.PAD;
            set
            {
                _flag |= ModifyForcedStatType.PAD;
                _forcedStat.PAD = value;
            }
        }

        public short PDD
        {
            get => _forcedStat.PDD;
            set
            {
                _flag |= ModifyForcedStatType.PDD;
                _forcedStat.PDD = value;
            }
        }

        public short MAD
        {
            get => _forcedStat.MAD;
            set
            {
                _flag |= ModifyForcedStatType.MAD;
                _forcedStat.MAD = value;
            }
        }

        public short MDD
        {
            get => _forcedStat.MDD;
            set
            {
                _flag |= ModifyForcedStatType.MDD;
                _forcedStat.MDD = value;
            }
        }

        public short ACC
        {
            get => _forcedStat.ACC;
            set
            {
                _flag |= ModifyForcedStatType.ACC;
                _forcedStat.ACC = value;
            }
        }

        public short EVA
        {
            get => _forcedStat.EVA;
            set
            {
                _flag |= ModifyForcedStatType.EVA;
                _forcedStat.EVA = value;
            }
        }

        public byte Speed
        {
            get => _forcedStat.Speed;
            set
            {
                _flag |= ModifyForcedStatType.Speed;
                _forcedStat.Speed = value;
            }
        }

        public byte Jump
        {
            get => _forcedStat.Jump;
            set
            {
                _flag |= ModifyForcedStatType.Jump;
                _forcedStat.Jump = value;
            }
        }

        public byte SpeedMax
        {
            get => _forcedStat.SpeedMax;
            set
            {
                _flag |= ModifyForcedStatType.SpeedMax;
                _forcedStat.SpeedMax = value;
            }
        }

        public ModifyForcedStatContext(ForcedStat forcedStat)
        {
            _forcedStat = forcedStat;
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<int>((int) _flag);

            if ((_flag & ModifyForcedStatType.STR) != 0) packet.Encode<short>(STR);
            if ((_flag & ModifyForcedStatType.DEX) != 0) packet.Encode<short>(DEX);
            if ((_flag & ModifyForcedStatType.INT) != 0) packet.Encode<short>(INT);
            if ((_flag & ModifyForcedStatType.LUK) != 0) packet.Encode<short>(LUK);
            if ((_flag & ModifyForcedStatType.PAD) != 0) packet.Encode<short>(PAD);
            if ((_flag & ModifyForcedStatType.PDD) != 0) packet.Encode<short>(PDD);
            if ((_flag & ModifyForcedStatType.MAD) != 0) packet.Encode<short>(MAD);
            if ((_flag & ModifyForcedStatType.MDD) != 0) packet.Encode<short>(MDD);
            if ((_flag & ModifyForcedStatType.ACC) != 0) packet.Encode<short>(ACC);
            if ((_flag & ModifyForcedStatType.EVA) != 0) packet.Encode<short>(EVA);
            if ((_flag & ModifyForcedStatType.Speed) != 0) packet.Encode<byte>(Speed);
            if ((_flag & ModifyForcedStatType.Jump) != 0) packet.Encode<byte>(Jump);
            if ((_flag & ModifyForcedStatType.SpeedMax) != 0) packet.Encode<byte>(SpeedMax);
        }
    }
}