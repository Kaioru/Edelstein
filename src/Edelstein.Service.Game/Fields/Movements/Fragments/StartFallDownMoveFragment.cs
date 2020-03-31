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
            _vPosition = packet.DecodePoint();
            _fallStartFoothold = packet.DecodeShort();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.EncodePoint(_vPosition);
            packet.EncodeShort(_fallStartFoothold);

            base.EncodeData(packet);
        }
    }
}