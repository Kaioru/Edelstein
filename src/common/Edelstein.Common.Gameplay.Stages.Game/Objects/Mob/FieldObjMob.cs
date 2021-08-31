﻿using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob
{
    public class FieldObjMob : AbstractFieldControlledLife, IFieldObjMob
    {
        public override FieldObjType Type => FieldObjType.Mob;

        public IFieldObjMobInfo Info { get; }
        public int HP { get; }
        public int MP { get; }

        public FieldObjMob(IFieldObjMobInfo info, bool left = true)
        {
            Info = info;
            Action = (MoveActionType)(byte)(
                Convert.ToByte(left) & 1 |
                2 * (byte)(Info.MoveAbility switch
                {
                    MoveAbilityType.Fly => MoveActionType.Fly1,
                    MoveAbilityType.Stop => MoveActionType.Stand,
                    _ => MoveActionType.Move,
                }
                )
            );

            HP = Info.MaxHP;
            MP = Info.MaxHP;
        }


        public void WriteData(IPacketWriter writer, FieldObjMobAppearType appearType, int? appearOption = null)
        {
            writer.WriteByte(1); // CalcDamageStatIndex
            writer.WriteInt(Info.ID);

            writer.WriteLong(0); // Temporary Stat
            writer.WriteLong(0); // Temporary Stat

            writer.WritePoint2D(Position);
            writer.WriteByte((byte)Action);
            writer.WriteShort((short)(Foothold?.ID ?? 0));
            writer.WriteShort(0); // Foothold again? TODO

            writer.WriteByte((byte)appearType);
            if (appearType == FieldObjMobAppearType.Revived ||
                appearType >= 0)
                writer.WriteInt(appearOption ?? 0);

            writer.WriteByte(0);
            writer.WriteInt(0);
            writer.WriteInt(0);
        }

        public IPacket GetEnterFieldPacket(FieldObjMobAppearType appearType, int? appearOption = null)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MobEnterField);

            packet.WriteInt(ID);
            WriteData(packet, appearType, appearOption);
            return packet;
        }

        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(FieldObjMobAppearType.Normal);

        public override IPacket GetLeaveFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MobLeaveField);

            packet.WriteInt(ID);
            packet.WriteInt(1); // m_tLastUpdateAmbush?
            return packet;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MobChangeController);

            packet.WriteBool(setAsController);
            packet.WriteInt(ID);

            if (setAsController)
                WriteData(packet, FieldObjMobAppearType.Regen);
            return packet;
        }

        public Task Hit(IFieldObjUser user, int damage)
        {
            throw new System.NotImplementedException();
        }

        public Task Kill(IFieldObjUser user)
        {
            throw new System.NotImplementedException();
        }
    }
}