using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Stats.Modify;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Stats.Modify
{
    public class ModifyForcedStatContext : IModifyForcedStatContext, IPacketWritable
    {
        public ModifyForcedStatType Flag { get; private set; }

        public short STR
        {
            get => _stats.STR ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.STR;
                _stats.STR = value;
            }
        }

        public short DEX
        {
            get => _stats.DEX ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.DEX;
                _stats.DEX = value;
            }
        }

        public short INT
        {
            get => _stats.INT ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.INT;
                _stats.INT = value;
            }
        }

        public short LUK
        {
            get => _stats.LUK ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.LUK;
                _stats.LUK = value;
            }
        }

        public short PAD
        {
            get => _stats.PAD ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.PAD;
                _stats.PAD = value;
            }
        }

        public short PDD
        {
            get => _stats.PDD ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.PDD;
                _stats.PDD = value;
            }
        }

        public short MAD
        {
            get => _stats.MAD ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.MAD;
                _stats.MAD = value;
            }
        }

        public short MDD
        {
            get => _stats.MDD ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.MDD;
                _stats.MDD = value;
            }
        }

        public short ACC
        {
            get => _stats.ACC ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.ACC;
                _stats.ACC = value;
            }
        }

        public short EVA
        {
            get => _stats.EVA ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.EVA;
                _stats.EVA = value;
            }
        }

        public byte Speed
        {
            get => _stats.Speed ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.Speed;
                _stats.Speed = value;
            }
        }

        public byte Jump
        {
            get => _stats.Jump ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.Jump;
                _stats.Jump = value;
            }
        }

        public byte SpeedMax
        {
            get => _stats.SpeedMax ?? 0;
            set
            {
                Flag |= ModifyForcedStatType.SpeedMax;
                _stats.SpeedMax = value;
            }
        }

        private readonly ForcedStats _stats;

        public ModifyForcedStatContext(ForcedStats stats)
        {
            _stats = stats;
        }

        public void Reset()
        {
            _stats.STR = null;
            _stats.DEX = null;
            _stats.INT = null;
            _stats.LUK = null;
            _stats.PAD = null;
            _stats.PDD = null;
            _stats.MAD = null;
            _stats.MDD = null;
            _stats.ACC = null;
            _stats.EVA = null;
            _stats.Speed = null;
            _stats.Jump = null;
            _stats.SpeedMax = null;

            Flag = 0;
        }

        public void WriteToPacket(IPacketWriter writer)
        {
            writer.WriteInt((int)Flag);

            if ((Flag & ModifyForcedStatType.STR) != 0) writer.WriteShort(STR);
            if ((Flag & ModifyForcedStatType.DEX) != 0) writer.WriteShort(DEX);
            if ((Flag & ModifyForcedStatType.INT) != 0) writer.WriteShort(INT);
            if ((Flag & ModifyForcedStatType.LUK) != 0) writer.WriteShort(LUK);
            if ((Flag & ModifyForcedStatType.PAD) != 0) writer.WriteShort(PAD);
            if ((Flag & ModifyForcedStatType.PDD) != 0) writer.WriteShort(PDD);
            if ((Flag & ModifyForcedStatType.MAD) != 0) writer.WriteShort(MAD);
            if ((Flag & ModifyForcedStatType.MDD) != 0) writer.WriteShort(MDD);
            if ((Flag & ModifyForcedStatType.ACC) != 0) writer.WriteShort(ACC);
            if ((Flag & ModifyForcedStatType.EVA) != 0) writer.WriteShort(EVA);
            if ((Flag & ModifyForcedStatType.Speed) != 0) writer.WriteByte(Speed);
            if ((Flag & ModifyForcedStatType.Jump) != 0) writer.WriteByte(Jump);
            if ((Flag & ModifyForcedStatType.SpeedMax) != 0) writer.WriteByte(SpeedMax);
        }
    }
}
