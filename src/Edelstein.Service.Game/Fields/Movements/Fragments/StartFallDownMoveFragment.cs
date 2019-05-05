using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class StartFallDownMoveFragment : ActionMoveFragment
    {
        public Point VPosition { get; set; }
        public short FallStartFoothold { get; set; }

        public StartFallDownMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void DecodeData(IPacket packet)
        {
            VPosition = packet.Decode<Point>();
            FallStartFoothold = packet.Decode<short>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(VPosition);
            packet.Encode<short>(FallStartFoothold);

            base.EncodeData(packet);
        }
    }
}