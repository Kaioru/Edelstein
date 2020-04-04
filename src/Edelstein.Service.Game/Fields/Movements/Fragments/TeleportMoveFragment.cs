using System.Drawing;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class TeleportMoveFragment : ActionMoveFragment
    {
        private Point _position;
        private short _foothold;

        public TeleportMoveFragment(MoveFragmentAttribute attribute, IPacketDecoder packet) : base(attribute, packet)
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
            _foothold = packet.DecodeShort();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodePoint(_position);
            packet.EncodeShort(_foothold);

            base.EncodeData(packet);
        }
    }
}