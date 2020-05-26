using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class StatChangeMoveFragment : AbstractMoveFragment
    {
        private bool _stat;

        public StatChangeMoveFragment(MoveFragmentAttribute attribute, IPacketDecoder packet) : base(attribute, packet)
        {
        }

        public override void Apply(IMoveContext context)
        {
            // Do nothing
        }

        public override void DecodeData(IPacketDecoder packet)
            => _stat = packet.DecodeBool();

        public override void EncodeData(IPacketEncoder packet)
            => packet.EncodeBool(_stat);
    }
}