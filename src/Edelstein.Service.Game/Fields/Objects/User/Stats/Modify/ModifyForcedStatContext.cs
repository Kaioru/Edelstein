using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Stats.Modify
{
    public class ModifyForcedStatContext
    {
        public ModifyForcedStatType Flag { get; set; }

        public short? STR
        {
            get => _forcedStat.STR;
            set
            {
                Flag |= ModifyForcedStatType.STR;
                _forcedStat.STR = value;
            }
        }

        public short? DEX
        {
            get => _forcedStat.DEX;
            set
            {
                Flag |= ModifyForcedStatType.DEX;
                _forcedStat.DEX = value;
            }
        }

        public short? INT
        {
            get => _forcedStat.INT;
            set
            {
                Flag |= ModifyForcedStatType.INT;
                _forcedStat.INT = value;
            }
        }

        public short? LUK
        {
            get => _forcedStat.LUK;
            set
            {
                Flag |= ModifyForcedStatType.LUK;
                _forcedStat.LUK = value;
            }
        }

        public short? PAD
        {
            get => _forcedStat.PAD;
            set
            {
                Flag |= ModifyForcedStatType.PAD;
                _forcedStat.PAD = value;
            }
        }

        public short? PDD
        {
            get => _forcedStat.PDD;
            set
            {
                Flag |= ModifyForcedStatType.PDD;
                _forcedStat.PDD = value;
            }
        }

        public short? MAD
        {
            get => _forcedStat.MAD;
            set
            {
                Flag |= ModifyForcedStatType.MAD;
                _forcedStat.MAD = value;
            }
        }

        public short? MDD
        {
            get => _forcedStat.MDD;
            set
            {
                Flag |= ModifyForcedStatType.MDD;
                _forcedStat.MDD = value;
            }
        }

        public short? ACC
        {
            get => _forcedStat.ACC;
            set
            {
                Flag |= ModifyForcedStatType.ACC;
                _forcedStat.ACC = value;
            }
        }

        public short? EVA
        {
            get => _forcedStat.EVA;
            set
            {
                Flag |= ModifyForcedStatType.EVA;
                _forcedStat.EVA = value;
            }
        }

        public byte? Speed
        {
            get => _forcedStat.Speed;
            set
            {
                Flag |= ModifyForcedStatType.Speed;
                _forcedStat.Speed = value;
            }
        }

        public byte? Jump
        {
            get => _forcedStat.Jump;
            set
            {
                Flag |= ModifyForcedStatType.Jump;
                _forcedStat.Jump = value;
            }
        }

        public byte? SpeedMax
        {
            get => _forcedStat.SpeedMax;
            set
            {
                Flag |= ModifyForcedStatType.SpeedMax;
                _forcedStat.SpeedMax = value;
            }
        }

        private readonly ForcedStat _forcedStat;

        public ModifyForcedStatContext(ForcedStat forcedStat)
            => _forcedStat = forcedStat;

        public void Encode(IPacket packet)
        {
            packet.EncodeInt((int) Flag);

            if ((Flag & ModifyForcedStatType.STR) != 0) packet.EncodeShort(STR ?? 0);
            if ((Flag & ModifyForcedStatType.DEX) != 0) packet.EncodeShort(DEX ?? 0);
            if ((Flag & ModifyForcedStatType.INT) != 0) packet.EncodeShort(INT ?? 0);
            if ((Flag & ModifyForcedStatType.LUK) != 0) packet.EncodeShort(LUK ?? 0);
            if ((Flag & ModifyForcedStatType.PAD) != 0) packet.EncodeShort(PAD ?? 0);
            if ((Flag & ModifyForcedStatType.PDD) != 0) packet.EncodeShort(PDD ?? 0);
            if ((Flag & ModifyForcedStatType.MAD) != 0) packet.EncodeShort(MAD ?? 0);
            if ((Flag & ModifyForcedStatType.MDD) != 0) packet.EncodeShort(MDD ?? 0);
            if ((Flag & ModifyForcedStatType.ACC) != 0) packet.EncodeShort(ACC ?? 0);
            if ((Flag & ModifyForcedStatType.EVA) != 0) packet.EncodeShort(EVA ?? 0);
            if ((Flag & ModifyForcedStatType.Speed) != 0) packet.EncodeByte(Speed ?? 0);
            if ((Flag & ModifyForcedStatType.Jump) != 0) packet.EncodeByte(Jump ?? 0);
            if ((Flag & ModifyForcedStatType.SpeedMax) != 0) packet.EncodeByte(SpeedMax ?? 0);
        }
    }
}