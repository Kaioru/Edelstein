using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class FlyingBlockMoveFragment : ActionMoveFragment
    {
        public Point Position { get; set; }
        public Point VPosition { get; set; }

        public FlyingBlockMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IFieldLife life)
        {
            base.Apply(life);

            life.Position = Position;
        }

        public override void DecodeData(IPacket packet)
        {
            Position = packet.Decode<Point>();
            VPosition = packet.Decode<Point>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(Position);
            packet.Encode<Point>(VPosition);

            base.EncodeData(packet);
        }
    }
}