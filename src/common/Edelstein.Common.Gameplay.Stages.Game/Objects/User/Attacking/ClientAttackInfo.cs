using System.Collections.Generic;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public class ClientAttackInfo : IPacketReadable
    {
        public int SkillID { get; private set; }
        public int Keydown { get; private set; }

        public int DamagePerMob { get; private set; }
        public int MobCount { get; private set; }

        public bool IsFinalAfterSlashBlast { get; private set; }
        public bool IsShadowPartner { get; private set; }
        public bool IsSerialAttack { get; private set; }

        public bool IsFacingLeft { get; private set; }

        public int Action { get; private set; }
        public byte ActionType { get; private set; }
        public byte ActionSpeed { get; private set; }
        public int ActionTime { get; private set; }

        public int Phase { get; private set; }

        public ICollection<ClientAttackMobInfo> Mobs { get; private set; }

        public ClientAttackInfo()
        {
            Mobs = new List<ClientAttackMobInfo>();
        }

        public void ReadFromPacket(IPacketReader reader)
        {
            _ = reader.ReadByte(); // FieldKey

            _ = reader.ReadInt(); // dr0
            _ = reader.ReadInt(); // dr1

            var attackInfo = reader.ReadByte();

            DamagePerMob = attackInfo >> 0 & 0xF;
            MobCount = attackInfo >> 4 & 0xF;

            _ = reader.ReadInt(); // dr2
            _ = reader.ReadInt(); // dr3

            SkillID = reader.ReadInt();
            _ = reader.ReadBool(); // unk

            _ = reader.ReadInt(); // dr rand
            _ = reader.ReadInt(); // crc

            _ = reader.ReadInt(); // skillLevel crc
            _ = reader.ReadInt(); // skillLevel crc

            Keydown = 0; // TODO keydownskill check - int keydown

            var skillFlags = reader.ReadByte();

            IsFinalAfterSlashBlast = (skillFlags & 0x0) > 0;
            IsShadowPartner = (skillFlags & 8) > 0;
            IsSerialAttack = (skillFlags & 32) > 0;

            var actionInfo = reader.ReadShort();

            Action = actionInfo & 0x7FFF;
            IsFacingLeft = (actionInfo >> 15 & 1) > 0;

            _ = reader.ReadInt(); // action crc?

            ActionType = reader.ReadByte();
            ActionSpeed = reader.ReadByte();
            ActionTime = reader.ReadInt();

            Phase = reader.ReadInt(); // BattleMage?

            for (var i = 0; i < MobCount; i++)
                Mobs.Add(reader.Read(new ClientAttackMobInfo(DamagePerMob)));

            _ = reader.ReadPoint2D(); // unk

            // TODO grenade readpoint2d
        }
    }

    public class ClientAttackMobInfo : IPacketReadable
    {
        public int MobID { get; private set; }

        public bool IsMobFacingLeft { get; private set; }

        public byte HitAction { get; private set; }
        public byte ForeAction { get; private set; }
        public byte FrameIdx { get; private set; }

        public int[] Damage { get; private set; }

        public short Delay { get; private set; }

        public ClientAttackMobInfo(int attackCount)
        {
            Damage = new int[attackCount];
        }

        public void ReadFromPacket(IPacketReader reader)
        {
            MobID = reader.ReadInt();

            HitAction = reader.ReadByte();

            var info = reader.ReadByte();

            ForeAction = (byte)(info & 0x7F);
            IsMobFacingLeft = (info >> 7 & 1) > 0;
            FrameIdx = reader.ReadByte();

            _ = reader.ReadByte(); // v258 & 0x7F | v25

            _ = reader.ReadShort(); // dAx[5]
            _ = reader.ReadShort(); // dAx[4]
            _ = reader.ReadShort(); // dAx[3]
            _ = reader.ReadShort(); // dAx[2]

            Delay = reader.ReadShort();

            for (var i = 0; i < Damage.Length; i++)
                Damage[i] = reader.ReadInt();

            _ = reader.ReadInt(); // mob crc
        }
    }
}