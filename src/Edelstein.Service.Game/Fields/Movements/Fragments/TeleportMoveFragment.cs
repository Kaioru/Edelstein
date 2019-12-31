using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class TeleportMoveFragment : ActionMoveFragment
    {
        private Point _position;
        private short _foothold;

        public TeleportMoveFragment(MoveFragmentAttribute attribute, IPacket packet) : base(attribute, packet)
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
            _foothold = packet.Decode<short>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(_position);
            packet.Encode<short>(_foothold);

            base.EncodeData(packet);
        }
    }
}