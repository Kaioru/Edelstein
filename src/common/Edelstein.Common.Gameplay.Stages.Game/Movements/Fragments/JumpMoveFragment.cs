using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class JumpMoveFragment : ActionMoveFragment
    {
        private Point2D _vPosition;

        public JumpMoveFragment(
            MoveFragmentAttribute attribute,
            IPacketReader reader
        ) : base(attribute, reader)
        {
        }

        public override void ReadData(IPacketReader reader)
        {
            _vPosition = reader.ReadPoint2D();

            base.ReadData(reader);
        }

        public override void WriteData(IPacketWriter writer)
        {
            writer.WritePoint2D(_vPosition);

            base.WriteData(writer);
        }
    }
}
