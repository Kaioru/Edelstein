using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Service.Game.Fields.Movements;

namespace Edelstein.Service.Game.Fields.Objects.Mobs
{
    public partial class FieldMob : AbstractFieldControlledLife
    {
        public override FieldObjType Type => FieldObjType.Mob;
        public MobTemplate Template { get; }
        public short HomeFoothold { get; set; }

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
        
        public Task OnPacket(RecvPacketOperations operation, IPacket packet)
        {
            return operation switch {
                RecvPacketOperations.MobMove => OnMobMove(packet),
                };
        }
    }
}