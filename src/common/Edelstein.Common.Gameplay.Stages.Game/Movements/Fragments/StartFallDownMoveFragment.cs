using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class StartFallDownMoveFragment : ActionMoveFragment
    {
        private Point2D _vPosition;
        private short _fallStartFootholdID;

        public StartFallDownMoveFragment(
            MoveFragmentAttribute attribute,
            IPacketReader reader
        ) : base(attribute, reader)
        {
        }

        protected override void ReadData(IPacketReader reader)
        {
            _vPosition = reader.ReadPoint2D();
            _fallStartFootholdID = reader.ReadShort();

            base.ReadData(reader);
        }

        protected override void WriteData(IPacketWriter writer)
        {
            writer.WritePoint2D(_vPosition);
            writer.WriteShort(_fallStartFootholdID);

            base.WriteData(writer);
        }
    }
}
