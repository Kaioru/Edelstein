using System.Drawing;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields.Movements.Fragments
{
    public class TeleportMoveFragment : ActionMoveFragment
    {
        public Point Position { get; set; }
        public short Foothold { get; set; }

        public TeleportMoveFragment(MovePathAttribute attribute, IPacket packet) : base(attribute, packet)
        {
        }

        public override void Apply(IFieldLife life)
        {
            base.Apply(life);

            life.Position = Position;
            life.Foothold = Foothold;
        }

        public override void DecodeData(IPacket packet)
        {
            Position = packet.Decode<Point>();
            Foothold = packet.Decode<short>();

            base.DecodeData(packet);
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<Point>(Position);
            packet.Encode<short>(Foothold);

            base.EncodeData(packet);
        }
    }
}