using System;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Templates.Mob;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Fields.Generators;
using Edelstein.Service.Game.Fields.Movements;

namespace Edelstein.Service.Game.Fields.Objects.Mob
{
    public class FieldMob : AbstractFieldControlledLife, IFieldGeneratorObj
    {
        public override FieldObjType Type => FieldObjType.Mob;

        public MobTemplate Template { get; }
        public short HomeFoothold { get; set; }
        public IFieldGenerator Generator { get; set; }

        public int HP { get; set; }
        public int MP { get; set; }

        public FieldMob(MobTemplate template, bool left = true)
        {
            Template = template;
            MoveAction = (byte) (
                Convert.ToByte(left) & 1 |
                2 * (byte) (Template.MoveAbility switch
                    {
                        MoveAbilityType.Fly => MoveActionType.Fly1,
                        MoveAbilityType.Stop => MoveActionType.Stand,
                        _ => MoveActionType.Move,
                    }
                )
            );

            HP = template.MaxHP;
            MP = template.MaxHP;
        }

        private void EncodeData(IPacketEncoder packet, MobAppearType summonType, int? summonOption = null)
        {
            packet.EncodeByte(1); // CalcDamageStatIndex
            packet.EncodeInt(Template.ID);

            packet.EncodeLong(0); // Temporary Stat
            packet.EncodeLong(0); // Temporary Stat

            packet.EncodePoint(Position);
            packet.EncodeByte(MoveAction);
            packet.EncodeShort(Foothold);
            packet.EncodeShort(HomeFoothold);

            packet.EncodeByte((byte) summonType);
            if (summonType == MobAppearType.Revived ||
                summonType >= 0)
                packet.EncodeInt(summonOption ?? 0);

            packet.EncodeByte(0);
            packet.EncodeInt(0);
            packet.EncodeInt(0);
        }

        public IPacket GetEnterFieldPacket(MobAppearType summonType, int? summonOption = null)
        {
            using var p = new OutPacket(SendPacketOperations.MobEnterField);
            p.EncodeInt(ID);
            EncodeData(p, summonType, summonOption);
            return p;
        }

        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(MobAppearType.Normal);

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new OutPacket(SendPacketOperations.MobLeaveField);
            p.EncodeInt(ID);
            p.EncodeByte(1); // m_tLastUpdateAmbush?
            return p;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using var p = new OutPacket(SendPacketOperations.MobChangeController);
            p.EncodeBool(setAsController);
            p.EncodeInt(ID);

            if (setAsController)
                EncodeData(p, MobAppearType.Regen);
            return p;
        }
    }
}