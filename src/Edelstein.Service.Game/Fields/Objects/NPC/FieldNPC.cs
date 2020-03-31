using System;
using Edelstein.Core.Templates.NPC;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.NPC
{
    public class FieldNPC : AbstractFieldControlledLife
    {
        public override FieldObjType Type => FieldObjType.NPC;

        public NPCTemplate Template { get; }

        public int RX0 { get; set; }
        public int RX1 { get; set; }

        public FieldNPC(NPCTemplate template, bool left = true)
        {
            Template = template;
            MoveAction = Convert.ToByte(left);
        }

        public override IPacket GetEnterFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.NpcEnterField);
            p.EncodeInt(ID);
            p.EncodeInt(Template.ID);

            p.EncodePoint(Position);
            p.EncodeByte(MoveAction);
            p.EncodeShort(Foothold);

            p.EncodeShort((short) RX0);
            p.EncodeShort((short) RX1);

            p.EncodeBool(true); // bEnabled
            return p;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.NpcLeaveField);
            p.EncodeInt(ID);
            return p;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using var p = new Packet(SendPacketOperations.NpcChangeController);
            p.EncodeBool(setAsController);
            p.EncodeInt(ID);
            return p;
        }
    }
}