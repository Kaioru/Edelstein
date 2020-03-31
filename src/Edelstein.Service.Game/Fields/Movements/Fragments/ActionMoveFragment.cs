using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class ActionMoveFragment : AbstractMoveFragment
    {
        private byte _moveAction;
        private short _elapse;

        public ActionMoveFragment(MoveFragmentAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IMoveContext context)
            => context.MoveAction = _moveAction;

        public override void DecodeData(IPacket packet)
        {
            _moveAction = packet.DecodeByte();
            _elapse = packet.DecodeShort();
        }

        public override void EncodeData(IPacket packet)
        {
            packet.EncodeByte(_moveAction);
            packet.EncodeShort(_elapse);
        }
    }
}