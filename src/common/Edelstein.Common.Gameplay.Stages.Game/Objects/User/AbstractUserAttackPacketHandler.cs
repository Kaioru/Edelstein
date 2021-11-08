using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Constants.Types;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Modify;
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

            var skill = (Skill)clientAttackInfo.SkillID;
            var skillLevel = clientAttackInfo.SkillID > 0 ? user.Character.GetSkillLevel(clientAttackInfo.SkillID) : 0;
            var damageType = Type == AttackType.Magic ? DamageType.Magic : DamageType.Physical;

            if (Type == AttackType.Melee)
            {
                // TODO bmage blows
            }

            if (Type == AttackType.Body)
            {
                // TODO teleport mastery
            }

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
                response.WriteInt(0); // bMovingShoot

                clientAttackInfo.Mobs.ForEach(m =>
                {
                    var critical = new bool[clientAttackInfo.DamagePerMob];
                    var damage = new int[clientAttackInfo.DamagePerMob];
                    var totalDamage = m.Damage.Sum();
                    var mob = user.Field.GetObject<IFieldObjMob>(m.MobID);

                    Array.Copy(m.Damage, damage, damage.Length);

                    response.WriteInt(m.MobID);
                    response.WriteByte(m.HitAction);

                    if (mob != null)
                    {
                        var equipInventory = user.Character.Inventories[ItemInventoryType.Equip];
                        var serverAttackInfo = new AttackInfo(user, mob)
                        {
                            WeaponID = equipInventory.Items.ContainsKey((short)BodyPart.Weapon)
                                ? equipInventory.Items[(short)BodyPart.Weapon].TemplateID
                                : 0,
                            BulletID = 0,
                            SkillID = clientAttackInfo.SkillID,
                            SkillLevel = skillLevel
                        };

                        var calculatedDamage = damageType == DamageType.Physical
                            ? user.Damage.CalculateCharacterPDamage(serverAttackInfo)
                            : user.Damage.CalculateCharacterMDamage(serverAttackInfo);
                        var calculatedTotalDamage = calculatedDamage.Select(d => d.Damage).Sum();

                        // TODO cheatdetector?
                        if (clientAttackInfo.DamagePerMob != calculatedDamage.Length)
                            user.Message($"Attack count mismatch: {clientAttackInfo.DamagePerMob} : {calculatedDamage.Length}");
                        if (totalDamage != calculatedTotalDamage)
                        {
                            user.Message($"Client damage: {string.Join(" + ", m.Damage.Select(m => $"{m}"))} = {totalDamage}");
                            user.Message($"Server damage: {string.Join(" + ", calculatedDamage.Select(m => $"{m.Damage}"))} = {calculatedTotalDamage}");
                        }

                        mob.Controller = user;
                        mob.Damage(user, calculatedTotalDamage);

                        for (var i = 0; i < clientAttackInfo.DamagePerMob; i++)
                        {
                            critical[i] = calculatedDamage[i].IsCritical;
                            damage[i] = calculatedDamage[i].Damage;
                        }
                    }
                    else user.Damage.SkipCalculationForCharacterDamage();

                    for (var i = 0; i < clientAttackInfo.DamagePerMob; i++)
                    {
                        response.WriteBool(critical[i]);
                        response.WriteInt(damage[i]);
                    }
                });
            }

            // TODO Keydown

            await user.FieldSplit.Dispatch(user, response);
        }
    }
}
