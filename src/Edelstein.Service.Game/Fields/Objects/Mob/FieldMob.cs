using System;
using System.Drawing;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Service.Game.Fields.Generators;
using Edelstein.Service.Game.Fields.Movements;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Fields.Objects.Mob
{
    public class FieldMob : AbstractFieldControlledLife, IFieldGeneratedObj
    {
        public override FieldObjType Type => FieldObjType.Mob;
        public MobTemplate Template { get; }
        public short HomeFoothold { get; set; }
        public IFieldGenerator Generator { get; set; }

        public int HP { get; set; }
        public int MP { get; set; }
        public int EXP { get; set; }

        public FieldMob(MobTemplate template, bool left = true)
        {
            Template = template;
            MoveAction = (byte) (
                Convert.ToByte(left) & 1 |
                2 * (byte) (Template.MoveAbility switch {
                    MoveAbilityType.Fly => MoveActionType.Fly,
                    MoveAbilityType.Stop => MoveActionType.Stand,
                    _ => MoveActionType.Move,
                    }
                )
            );

            HP = template.MaxHP;
            MP = template.MaxHP;
            EXP = template.EXP;
        }

        public void Damage(IFieldObj source, int damage)
        {
            lock (this)
            {
                HP -= damage;

                if (source is FieldUser user)
                {
                    var indicator = HP / (float) Template.MaxHP * 100f;

                    indicator = Math.Min(100, indicator);
                    indicator = Math.Max(0, indicator);
                    using (var p = new Packet(SendPacketOperations.MobHPIndicator))
                    {
                        p.Encode<int>(ID);
                        p.Encode<byte>((byte) indicator);
                        user.SendPacket(p);
                    }
                }

                if (HP <= 0) Field.Leave(this, source);
            }
        }

        private void EncodeData(IPacket packet, MobAppearType summonType, int? summonOption = null)
        {
            packet.Encode<byte>(1); // CalcDamageStatIndex
            packet.Encode<int>(Template.ID);

            packet.Encode<long>(0); // Temporary Stat
            packet.Encode<long>(0); // Temporary Stat

            packet.Encode<Point>(Position);
            packet.Encode<byte>(MoveAction);
            packet.Encode<short>(Foothold);
            packet.Encode<short>(HomeFoothold);

            packet.Encode<sbyte>((sbyte) summonType);
            if (summonType == MobAppearType.Revived ||
                summonType >= 0)
                packet.Encode<int>(summonOption ?? 0);

            packet.Encode<byte>(0);
            packet.Encode<int>(0);
            packet.Encode<int>(0);
        }
        public IPacket GetEnterFieldPacket(MobAppearType summonType, int? summonOption = null)
        {
            using (var p = new Packet(SendPacketOperations.MobEnterField))
            {
                p.Encode<int>(ID);
                EncodeData(p, summonType, summonOption);
                return p;
            }
        }


        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(MobAppearType.Normal);

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.MobLeaveField))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(1); // m_tLastUpdateAmbush?
                return p;
            }
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using (var p = new Packet(SendPacketOperations.MobChangeController))
            {
                p.Encode<bool>(setAsController);
                p.Encode<int>(ID);

                if (setAsController)
                    EncodeData(p, MobAppearType.Regen);
                return p;
            }
        }
    }
}