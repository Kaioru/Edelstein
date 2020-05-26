using System.Drawing;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class NormalMoveFragment : ActionMoveFragment
    {
        private Point _position;
        private Point _vPosition;
        private short _foothold;
        private short _fallStartFoothold;
        private Point _offset;

        public NormalMoveFragment(MoveFragmentAttribute attribute, IPacketDecoder packet) : base(attribute, packet)
        {
        }

        public override void Apply(IMoveContext context)
        {
            base.Apply(context);

            context.Position = _position;
            context.Foothold = _foothold;
        }

        public override void DecodeData(IPacketDecoder packet)
        {
            _position = packet.DecodePoint();
            _vPosition = packet.DecodePoint();
            _foothold = packet.DecodeShort();
            if (Attribute == MoveFragmentAttribute.FallDown)
                _fallStartFoothold = packet.DecodeShort();
            _offset = packet.DecodePoint();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodePoint(_position);
            packet.EncodePoint(_vPosition);
            packet.EncodeShort(_foothold);
            if (Attribute == MoveFragmentAttribute.FallDown)
                packet.EncodeShort(_fallStartFoothold);
            packet.EncodePoint(_offset);

            base.EncodeData(packet);
        }
    }
}