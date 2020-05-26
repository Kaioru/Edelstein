using System.Drawing;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class FlyingBlockMoveFragment : ActionMoveFragment
    {
        private Point _position;
        private Point _vPosition;

        public FlyingBlockMoveFragment(MoveFragmentAttribute attribute, IPacketDecoder packet) : base(attribute, packet)
        {
        }

        public override void Apply(IMoveContext context)
        {
            base.Apply(context);

            context.Position = _position;
        }

        public override void DecodeData(IPacketDecoder packet)
        {
            _position = packet.DecodePoint();
            _vPosition = packet.DecodePoint();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodePoint(_position);
            packet.EncodePoint(_vPosition);

            base.EncodeData(packet);
        }
    }
}