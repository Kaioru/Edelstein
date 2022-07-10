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

        public void WriteData(IPacketWriter writer)
        {
            writer.WriteInt(Info.ID);

            writer.WritePoint2D(Position);
            writer.WriteByte((byte)Action);
            writer.WriteShort((short)(Foothold?.ID ?? 0));

            writer.WriteShort((short)RX0);
            writer.WriteShort((short)RX1);

            writer.WriteBool(true); // enabled
        }

        public override IPacket GetEnterFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.NpcEnterField);

            packet.WriteInt(ID);

            WriteData(packet);
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
                WriteData(packet);
            return packet;
        }

        public Task Talk(IFieldObjUser user) { throw new NotImplementedException(); }
    }
}
