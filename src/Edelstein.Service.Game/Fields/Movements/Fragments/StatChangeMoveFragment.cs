using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class StatChangeMoveFragment : AbstractMoveFragment
    {
        private bool Stat { get; set; }

        public StatChangeMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IFieldLife life)
        {
        }

        public override void DecodeData(IPacket packet)
        {
            Stat = packet.Decode<bool>();
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<bool>(Stat);
        }
    }
}