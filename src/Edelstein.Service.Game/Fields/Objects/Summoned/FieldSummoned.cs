using System.Drawing;
using Edelstein.Core;
using Edelstein.Core.Types;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Service.Game.Fields.Movements;
using Edelstein.Service.Game.Fields.Objects.User.Attacking;

namespace Edelstein.Service.Game.Fields.Objects.Summoned
{
    public class FieldSummoned : AbstractFieldLife, IFieldOwnedObj
    {
        public override FieldObjType Type => FieldObjType.Summoned;
        public IFieldUser Owner { get; }

        public MoveAbilityType MoveAbility { get; }
        public AttackType AssistType { get; }
        public int SkillID { get; }
        public byte SkillLevel { get; }

        public FieldSummoned(IFieldUser owner, int skillID, byte skillLevel)
        {
            Owner = owner;
            SkillID = skillID;
            SkillLevel = skillLevel;

            MoveAbility = MoveAbilityType.Fly;
            AssistType = AttackType.Shoot;

            switch ((Skill) skillID)
            {
                case Skill.DarkknightBeholder:
                    MoveAbility = MoveAbilityType.Walk;
                    AssistType = AttackType.Melee;
                    break;
                case Skill.Archmage1Ifrit:
                case Skill.Archmage2Elquines:
                case Skill.BishopBahamut:
                    MoveAbility = MoveAbilityType.Walk;
                    break;
                case Skill.RangerPuppet:
                case Skill.SniperPuppet:
                    MoveAbility = MoveAbilityType.Stop;
                    AssistType = AttackType.Melee;
                    break;
            }

            Position = owner.Position;
            Foothold = (short) (MoveAbility == MoveAbilityType.Fly ? 0 : owner.Foothold);
            MoveAction = (byte) MoveActionType.Alert;
        }

        public IPacket GetEnterFieldPacket(byte enterType)
        {
            using (var p = new Packet(SendPacketOperations.SummonedEnterField))
            {
                p.Encode<int>(Owner.ID);
                p.Encode<int>(ID);
                p.Encode<int>(SkillID);
                p.Encode<byte>(Owner.Character.Level);
                p.Encode<byte>(SkillLevel);
                p.Encode<Point>(Position);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);
                p.Encode<byte>((byte) MoveAbility);
                p.Encode<byte>((byte) AssistType);
                p.Encode<byte>(enterType);

                p.Encode<bool>(false); // Mirror Image avatarlook

                return p;
            }
        }

        public IPacket GetLeaveFieldPacket(byte leaveType)
        {
            using (var p = new Packet(SendPacketOperations.SummonedLeaveField))
            {
                p.Encode<int>(Owner.ID);
                p.Encode<int>(ID);
                p.Encode<byte>(leaveType);
                return p;
            }
        }

        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(0x0);

        public override IPacket GetLeaveFieldPacket()
            => GetLeaveFieldPacket(0x1);
    }
}