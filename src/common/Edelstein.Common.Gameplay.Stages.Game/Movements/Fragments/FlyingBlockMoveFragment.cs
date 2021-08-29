using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class FlyingBlockMoveFragment : ActionMoveFragment
    {
        private Point2D _position;
        private Point2D _vPosition;

        public FlyingBlockMoveFragment(
            MoveFragmentAttribute attribute,
            IPacketReader reader
        ) : base(attribute, reader)
        {
        }

        public override void Apply(MovePath path)
        {
            base.Apply(path);

            path.Position = _position;
        }

        protected override void ReadData(IPacketReader reader)
        {
            _position = reader.ReadPoint2D();
            _vPosition = reader.ReadPoint2D();

            base.ReadData(reader);
        }

        protected override void WriteData(IPacketWriter writer)
        {
            writer.WritePoint2D(_position);
            writer.WritePoint2D(_vPosition);

            base.WriteData(writer);
        }
    }
}
