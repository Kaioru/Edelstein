using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Types;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Attacking;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserAttackHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var type = (AttackType) (operation - RecvPacketOperations.UserMeleeAttack);
            var info = new AttackInfo(type, user, packet);

            // keydown packets

            using (var p = new Packet(SendPacketOperations.UserMeleeAttack + (int) type))
            {
                p.Encode<int>(user.ID);
                p.Encode<byte>((byte) (info.DamagePerMob | 16 * info.MobCount));
                p.Encode<byte>(user.Character.Level);

                if (info.SkillID > 0)
                {
                    p.Encode<byte>((byte) user.Character.GetSkillLevel(info.SkillID));
                    p.Encode<int>(info.SkillID);
                }
                else p.Encode<byte>(0);

                // 3211006 check

                p.Encode<byte>(0x20); // bSerialAttack
                p.Encode<short>((short) (info.Action & 0x7FFF | (Convert.ToInt16(info.Left) << 15)));

                if (info.Action <= 0x110)
                {
                    p.Encode<byte>(0); // nMastery
                    p.Encode<byte>(0); // v82
                    p.Encode<int>(2070000); // bMovingShoot

                    info.DamageInfo.ForEach(i =>
                    {
                        p.Encode<int>(i.MobID);

                        if (i.MobID <= 0) return;

                        p.Encode<byte>(i.HitAction);

                        // check 4211006

                        i.Damage.ForEach(d =>
                        {
                            p.Encode<bool>(false);
                            p.Encode<int>(d);
                        });
                    });
                }

                if (type == AttackType.Shoot)
                {
                    p.Encode<short>(0); // bSerialAttack?
                    p.Encode<short>(0); // v91
                }

                switch ((Skill) info.SkillID)
                {
                    case Skill.Archmage1Bigbang:
                    case Skill.Archmage2Bigbang:
                    case Skill.BishopBigbang:
                    case Skill.EvanIceBreath:
                    case Skill.EvanBreath:
                        p.Encode<int>(info.KeyDown);
                        break;
                }

                await user.Field.BroadcastPacket(user, p);
            }

            await info.Apply();
        }
    }
}