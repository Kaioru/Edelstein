using System;
using System.Drawing;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life.NPC;

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
            p.Encode<int>(ID);
            p.Encode<int>(Template.ID);

            p.Encode<Point>(Position);
            p.Encode<byte>(MoveAction);
            p.Encode<short>(Foothold);

            p.Encode<short>((short) RX0);
            p.Encode<short>((short) RX1);

            p.Encode<bool>(true); // bEnabled
            return p;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new Packet(SendPacketOperations.NpcLeaveField);
            p.Encode<int>(ID);
            return p;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using var p = new Packet(SendPacketOperations.NpcChangeController);
            p.Encode<bool>(setAsController);
            p.Encode<int>(ID);
            return p;
        }
    }
}