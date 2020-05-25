using System;
using Edelstein.Core.Templates.NPC;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Generators;

namespace Edelstein.Service.Game.Fields.Objects.NPC
{
    public class FieldNPC : AbstractFieldControlledLife, IFieldGeneratorObj
    {
        public override FieldObjType Type => FieldObjType.NPC;

        public NPCTemplate Template { get; }
        public IFieldGenerator Generator { get; set; }

        public int RX0 { get; set; }
        public int RX1 { get; set; }
        public bool Enabled { get; set; }

        public FieldNPC(NPCTemplate template, bool left = true)
        {
            Template = template;
            MoveAction = Convert.ToByte(left);
        }

        public override IPacket GetEnterFieldPacket()
        {
            using var p = new OutPacket(SendPacketOperations.NpcEnterField);
            p.EncodeInt(ID);
            p.EncodeInt(Template.ID);

            p.EncodePoint(Position);
            p.EncodeByte(MoveAction);
            p.EncodeShort(Foothold);

            p.EncodeShort((short) RX0);
            p.EncodeShort((short) RX1);

            p.EncodeBool(!Enabled);
            return p;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new OutPacket(SendPacketOperations.NpcLeaveField);
            p.EncodeInt(ID);
            return p;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using var p = new OutPacket(SendPacketOperations.NpcChangeController);
            p.EncodeBool(setAsController);
            p.EncodeInt(ID);
            return p;
        }
    }
}