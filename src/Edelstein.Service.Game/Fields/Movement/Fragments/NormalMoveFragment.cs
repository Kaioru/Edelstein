using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movement.Fragments
{
    public class NormalMoveFragment : ActionMoveFragment
    {
        public Point Position { get; set; }
        public Point VPosition { get; set; }
        public short Foothold { get; set; }
        public short FallStartFoothold { get; set; }
        public Point Offset { get; set; }

        public NormalMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IFieldLife life)
        {
            base.Apply(life);

            life.Position = Position;
            life.Foothold = Foothold;
        }

        public override void DecodeData(IPacket packet)
        {
            Position = packet.Decode<Point>();
            VPosition = packet.Decode<Point>();
            Foothold = packet.Decode<short>();
            if (Attribute == MovePathAttribute.FallDown)
                FallStartFoothold = packet.Decode<short>();
            Offset = packet.Decode<Point>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(Position);
            packet.Encode<Point>(VPosition);
            packet.Encode<short>(Foothold);
            if (Attribute == MovePathAttribute.FallDown)
                packet.Encode<short>(FallStartFoothold);
            packet.Encode<Point>(Offset);

            base.EncodeData(packet);
        }
    }
}