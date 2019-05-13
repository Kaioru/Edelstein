using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.Mobs;

namespace Edelstein.Service.Game.Fields.User.Attacking
{
    public class DamageInfo
    {
        private readonly AttackType _type;
        private readonly FieldUser _user;

        public int MobID { get; private set; }
        public byte HitAction { get; private set; }
        public int ForeAction { get; private set; }
        public bool Left { get; private set; }
        public byte FrameIdx { get; private set; }
        public int CalcDamageStatIndex { get; private set; }
        public bool Doomed { get; private set; }
        public Point HitPosition { get; private set; }
        public Point PrevPosition { get; private set; }
        public short Delay { get; private set; }

        public int[] Damage { get; private set; }

        public DamageInfo(AttackType type, FieldUser user)
        {
            _type = type;
            _user = user;
        }

        public void Decode(IPacket packet, int damagePerMob)
        {
            MobID = packet.Decode<int>();
            HitAction = packet.Decode<byte>();
            var v37 = packet.Decode<byte>();
            ForeAction = v37 & 0x7F;
            Left = Convert.ToBoolean((v37 >> 7) & 1);
            FrameIdx = packet.Decode<byte>();
            var v38 = packet.Decode<byte>();
            CalcDamageStatIndex = v38 & 0x7F;
            Doomed = Convert.ToBoolean((v38 >> 7) & 1);
            HitPosition = packet.Decode<Point>();
            PrevPosition = packet.Decode<Point>();

            // if 40413B + 3
            // else
            Delay = packet.Decode<short>();

            Damage = new int[damagePerMob];
            for (var i = 0; i < damagePerMob; i++)
                Damage[i] = packet.Decode<int>();

            packet.Decode<int>();
        }

        public async Task Apply()
        {
            var mob = _user.Field.GetObject<FieldMob>(MobID);
            var totalDamage = Damage.Sum();

            if (mob != null)
            {
                mob.Controller = _user;
                mob.Damage(_user, totalDamage);
            }
        }
    }
}