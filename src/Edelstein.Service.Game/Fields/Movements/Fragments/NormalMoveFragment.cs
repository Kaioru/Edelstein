using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class NormalMoveFragment : ActionMoveFragment
    {
        private Point _position;
        private Point _vPosition;
        private short _foothold;
        private short _fallStartFoothold;
        private Point _offset;

        public NormalMoveFragment(MoveFragmentAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IMoveContext context)
        {
            base.Apply(context);

            context.Position = _position;
            context.Foothold = _foothold;
        }

        public override void DecodeData(IPacket packet)
        {
            _position = packet.Decode<Point>();
            _vPosition = packet.Decode<Point>();
            _foothold = packet.Decode<short>();
            if (Attribute == MoveFragmentAttribute.FallDown)
                _fallStartFoothold = packet.Decode<short>();
            _offset = packet.Decode<Point>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(_position);
            packet.Encode<Point>(_vPosition);
            packet.Encode<short>(_foothold);
            if (Attribute == MoveFragmentAttribute.FallDown)
                packet.Encode<short>(_fallStartFoothold);
            packet.Encode<Point>(_offset);

            base.EncodeData(packet);
        }
    }
}