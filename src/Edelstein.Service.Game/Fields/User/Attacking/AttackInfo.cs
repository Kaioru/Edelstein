using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Attacking
{
    public class AttackInfo
    {
        private readonly AttackType _type;
        private readonly FieldUser _user;

        public int DamagePerMob { get; private set; }
        public int MobCount { get; private set; }
        public int SkillID { get; private set; }
        public int KeyDown { get; private set; }
        public byte Option { get; private set; }
        public bool Left { get; private set; }
        public int Action { get; private set; }
        public byte AttackActionType { get; private set; }
        public byte AttackSpeed { get; private set; }
        public int AttackTime { get; private set; }

        public DamageInfo[] DamageInfo { get; private set; }

        public AttackInfo(AttackType type, FieldUser user, IPacket packet)
        {
            _type = type;
            _user = user;
            
            Decode(packet);
        }

        private void Decode(IPacket packet)
        {
            packet.Decode<byte>(); // fieldKey

            packet.Decode<int>(); // pDrInfo
            packet.Decode<int>(); // pDrInfo

            var v6 = packet.Decode<byte>();
            DamagePerMob = v6 & 0xF;
            MobCount = v6 >> 4;

            packet.Decode<int>(); // pDrInfo
            packet.Decode<int>(); // pDrInfo

            SkillID = packet.Decode<int>();
            packet.Decode<byte>(); // combatOrders

            if (_type == AttackType.Magic)
            {
                packet.Decode<int>();
                packet.Decode<int>();
                packet.Decode<int>();
                packet.Decode<int>();
                packet.Decode<int>();
                packet.Decode<int>();
            }
            
            packet.Decode<int>(); // rand
            packet.Decode<int>(); // crc?
            
            packet.Decode<int>();
            packet.Decode<int>();

            if (SkillConstants.IsKeydownSkill(SkillID))
                KeyDown = packet.Decode<int>();

            Option = packet.Decode<byte>();

            var v17 = packet.Decode<short>();
            Left = Convert.ToBoolean((v17 >> 15) & 1);
            Action = v17 & 0x7FFF;

            packet.Decode<int>();

            AttackActionType = packet.Decode<byte>();
            AttackSpeed = packet.Decode<byte>();
            AttackTime = packet.Decode<int>();

            packet.Decode<int>();

            DamageInfo = new DamageInfo[MobCount];
            for (var i = 0; i < MobCount; i++)
            {
                var info = new DamageInfo(_type, _user);

                info.Decode(packet, DamagePerMob);
                DamageInfo[i] = info;
            }
        }

        public async Task Apply()
            => await Task.WhenAll(DamageInfo.Select(i => i.Apply()));
    }
}