using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class TeleportMoveFragment : ActionMoveFragment
    {
        private Point2D _position;
        private short _footholdID;

        public TeleportMoveFragment(
            MoveFragmentAttribute attribute,
            IPacketReader reader
        ) : base(attribute, reader)
        {
        }

        public override void Apply(MovePath path)
        {
            base.Apply(path);

            path.Position = _position;
            path.FootholdID = _footholdID;
        }

        protected override void ReadData(IPacketReader reader)
        {
            _position = reader.ReadPoint2D();
            _footholdID = reader.ReadShort();

            base.ReadData(reader);
        }

        protected override void WriteData(IPacketWriter writer)
        {
            writer.WritePoint2D(_position);
            writer.WriteShort(_footholdID);

            base.WriteData(writer);
        }
    }
}
