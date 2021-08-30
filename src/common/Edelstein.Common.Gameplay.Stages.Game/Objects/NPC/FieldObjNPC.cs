using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC
{
    public class FieldObjNPC : AbstractFieldControlledLife, IFieldObjNPC
    {
        public override FieldObjType Type => FieldObjType.NPC;

        public IFieldObjNPCInfo Info { get; }

        public int RX0 { get; set; }
        public int RX1 { get; set; }

        public FieldObjNPC(IFieldObjNPCInfo info, bool left = true)
        {
            Info = info;
            Action = (MoveActionType)Convert.ToByte(left);
        }

        public override IPacket GetEnterFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.NpcEnterField);

            packet.WriteInt(ID);
            packet.WriteInt(Info.ID);

            packet.WritePoint2D(Position);
            packet.WriteByte((byte)Action);
            packet.WriteShort((short)(Foothold?.ID ?? 0));

            packet.WriteShort((short)RX0);
            packet.WriteShort((short)RX1);

            packet.WriteBool(true); // enabled
            return packet;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.NpcLeaveField);

            packet.WriteInt(ID);
            return packet;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.NpcChangeController);

            packet.WriteBool(setAsController);
            packet.WriteInt(ID);

            if (setAsController)
            {
                // TODO: investigate why this triggers
                packet.WriteInt(Info.ID);
                packet.WritePoint2D(Position);
                packet.WriteByte((byte)Action);
                packet.WriteShort((short)(Foothold?.ID ?? 0));

                packet.WriteShort((short)RX0);
                packet.WriteShort((short)RX1);

                packet.WriteBool(true);
            }
            return packet;
        }

        public Task Talk(IFieldObjUser user) { throw new NotImplementedException(); }
    }
}
