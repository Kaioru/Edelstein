using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User
{
    public abstract class AbstractUserAttackPacketHandler : AbstractUserPacketHandler
    {
        public abstract AttackType Type { get; }

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var clientAttackInfo = packet.Read(new ClientAttackInfo());
            var skillLevel = clientAttackInfo.SkillID > 0 ? user.Character.GetSkillLevel(clientAttackInfo.SkillID) : 0;

            var operation = (PacketSendOperations)((int)PacketSendOperations.UserMeleeAttack + (int)Type);
            var response = new UnstructuredOutgoingPacket(operation);

            response.WriteInt(user.ID);
            response.WriteByte((byte)(clientAttackInfo.DamagePerMob | 16 * clientAttackInfo.MobCount));
            response.WriteByte(user.Character.Level);

            response.WriteByte((byte)skillLevel);
            if (skillLevel > 0)
                response.WriteInt(clientAttackInfo.SkillID);

            response.WriteByte((byte)(
                1 * Convert.ToByte(clientAttackInfo.IsFinalAfterSlashBlast) |
                8 * Convert.ToByte(clientAttackInfo.IsShadowPartner) |
                16 * 0 |
                32 * Convert.ToByte(clientAttackInfo.IsSerialAttack)
            ));
            response.WriteShort((short)(
                clientAttackInfo.Action & 0x7FFF |
                Convert.ToByte(clientAttackInfo.IsFacingLeft) << 15)
            );

            if (clientAttackInfo.Action <= 0x110)
            {
                response.WriteByte(0); // nMastery
                response.WriteByte(0); // v82
                response.WriteInt(2070000); // bMovingShoot

                clientAttackInfo.Mobs.ForEach(m =>
                {
                    // TODO: temporary
                    var mob = user.Field.GetObject<IFieldObjMob>(m.MobID);

                    if (mob != null)
                        mob.Controller = user;

                    response.WriteInt(m.MobID);
                    response.WriteByte(m.HitAction);

                    // TODO use calcdamage
                    m.Damage.ForEach(d =>
                    {
                        response.WriteBool(false); // Critical
                        response.WriteInt(d);
                    });
                });
            }

            // TODO Keydown

            await user.FieldSplit.Dispatch(user, response);
        }
    }
}
