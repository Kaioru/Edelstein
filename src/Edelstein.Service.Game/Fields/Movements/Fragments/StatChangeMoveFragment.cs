using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class StatChangeMoveFragment : AbstractMoveFragment
    {
        private bool _stat;

        public StatChangeMoveFragment(MoveFragmentAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IMoveContext context)
        {
            // Do nothing
        }

        public override void DecodeData(IPacket packet)
            => _stat = packet.Decode<bool>();

        public override void EncodeData(IPacket packet)
            => packet.Encode<bool>(_stat);
    }
}