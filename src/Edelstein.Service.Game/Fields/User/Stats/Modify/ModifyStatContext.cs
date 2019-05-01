using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Stats.Modify
{
    public class ModifyStatContext
    {
        private readonly Character _character;

        public ModifyStatType Flag { get; set; }

        public byte Skin
        {
            get => _character.Skin;
            set
            {
                Flag |= ModifyStatType.Skin;
                _character.Skin = value;
            }
        }

        public int Face
        {
            get => _character.Face;
            set
            {
                Flag |= ModifyStatType.Face;
                _character.Face = value;
            }
        }

        public int Hair
        {
            get => _character.Hair;
            set
            {
                Flag |= ModifyStatType.Hair;
                _character.Hair = value;
            }
        }

        // TODO: Pet stuff

        public byte Level
        {
            get => _character.Level;
            set
            {
                Flag |= ModifyStatType.Level;
                _character.Level = value;
            }
        }

        public short Job
        {
            get => _character.Job;
            set
            {
                Flag |= ModifyStatType.Job;
                _character.Job = value;
            }
        }

        public short STR
        {
            get => _character.STR;
            set
            {
                Flag |= ModifyStatType.STR;
                _character.STR = value;
            }
        }

        public short DEX
        {
            get => _character.DEX;
            set
            {
                Flag |= ModifyStatType.DEX;
                _character.DEX = value;
            }
        }

        public short INT
        {
            get => _character.INT;
            set
            {
                Flag |= ModifyStatType.INT;
                _character.INT = value;
            }
        }

        public short LUK
        {
            get => _character.LUK;
            set
            {
                Flag |= ModifyStatType.LUK;
                _character.LUK = value;
            }
        }

        public int HP
        {
            get => _character.HP;
            set
            {
                Flag |= ModifyStatType.HP;
                _character.HP = value;
            }
        }

        public int MaxHP
        {
            get => _character.MaxHP;
            set
            {
                Flag |= ModifyStatType.MaxHP;
                _character.MaxHP = value;
            }
        }

        public int MP
        {
            get => _character.MP;
            set
            {
                Flag |= ModifyStatType.MP;
                _character.MP = value;
            }
        }

        public int MaxMP
        {
            get => _character.MaxMP;
            set
            {
                Flag |= ModifyStatType.MaxMP;
                _character.MaxMP = value;
            }
        }

        public short AP
        {
            get => _character.AP;
            set
            {
                Flag |= ModifyStatType.AP;
                _character.AP = value;
            }
        }

        public short SP
        {
            get => _character.SP;
            set
            {
                Flag |= ModifyStatType.SP;
                _character.SP = value;
            }
        }

        public int EXP
        {
            get => _character.EXP;
            set
            {
                Flag |= ModifyStatType.EXP;
                _character.EXP = value;
            }
        }

        public short POP
        {
            get => _character.POP;
            set
            {
                Flag |= ModifyStatType.POP;
                _character.POP = value;
            }
        }

        public int Money
        {
            get => _character.Money;
            set
            {
                Flag |= ModifyStatType.Money;
                _character.Money = value;
            }
        }

        public int TempEXP
        {
            get => _character.TempEXP;
            set
            {
                Flag |= ModifyStatType.TempEXP;
                _character.TempEXP = value;
            }
        }

        public ModifyStatContext(Character character)
        {
            _character = character;
        }

        public void Encode(IPacket packet)
        {
            packet.Encode<int>((int) Flag);

            if ((Flag & ModifyStatType.Skin) != 0) packet.Encode<byte>(Skin);
            if ((Flag & ModifyStatType.Face) != 0) packet.Encode<int>(Face);
            if ((Flag & ModifyStatType.Hair) != 0) packet.Encode<int>(Hair);

            if ((Flag & ModifyStatType.Pet) != 0) packet.Encode<long>(0);
            if ((Flag & ModifyStatType.Pet2) != 0) packet.Encode<long>(0);
            if ((Flag & ModifyStatType.Pet3) != 0) packet.Encode<long>(0);

            if ((Flag & ModifyStatType.Level) != 0) packet.Encode<byte>(Level);
            if ((Flag & ModifyStatType.Job) != 0) packet.Encode<short>(Job);
            if ((Flag & ModifyStatType.STR) != 0) packet.Encode<short>(STR);
            if ((Flag & ModifyStatType.DEX) != 0) packet.Encode<short>(DEX);
            if ((Flag & ModifyStatType.INT) != 0) packet.Encode<short>(INT);
            if ((Flag & ModifyStatType.LUK) != 0) packet.Encode<short>(LUK);

            if ((Flag & ModifyStatType.HP) != 0) packet.Encode<int>(HP);
            if ((Flag & ModifyStatType.MaxHP) != 0) packet.Encode<int>(MaxHP);
            if ((Flag & ModifyStatType.MP) != 0) packet.Encode<int>(MP);
            if ((Flag & ModifyStatType.MaxMP) != 0) packet.Encode<int>(MaxMP);

            if ((Flag & ModifyStatType.AP) != 0) packet.Encode<short>(AP);
            if ((Flag & ModifyStatType.SP) != 0)
            {
                if (Job / 1000 != 3 && Job / 100 != 22 && Job != 2001)
                    packet.Encode<short>(SP);
                else packet.Encode<byte>(0);
            }

            if ((Flag & ModifyStatType.EXP) != 0) packet.Encode<int>(EXP);
            if ((Flag & ModifyStatType.POP) != 0) packet.Encode<short>(POP);

            if ((Flag & ModifyStatType.Money) != 0) packet.Encode<int>(Money);
            if ((Flag & ModifyStatType.TempEXP) != 0) packet.Encode<int>(TempEXP);
        }
    }
}