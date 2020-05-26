using System.Drawing;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class StartFallDownMoveFragment : ActionMoveFragment
    {
        private Point _vPosition;
        private short _fallStartFoothold;

        public StartFallDownMoveFragment(MoveFragmentAttribute attribute, IPacketDecoder packet) : base(attribute, packet)
        {
        }

        public override void DecodeData(IPacketDecoder packet)
        {
            _vPosition = packet.DecodePoint();
            _fallStartFoothold = packet.DecodeShort();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodePoint(_vPosition);
            packet.EncodeShort(_fallStartFoothold);

            base.EncodeData(packet);
        }
    }
}