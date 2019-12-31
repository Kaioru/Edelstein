using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class StartFallDownMoveFragment : ActionMoveFragment
    {
        private Point _vPosition;
        private short _fallStartFoothold;

        public StartFallDownMoveFragment(MoveFragmentAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void DecodeData(IPacket packet)
        {
            _vPosition = packet.Decode<Point>();
            _fallStartFoothold = packet.Decode<short>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(_vPosition);
            packet.Encode<short>(_fallStartFoothold);

            base.EncodeData(packet);
        }
    }
}