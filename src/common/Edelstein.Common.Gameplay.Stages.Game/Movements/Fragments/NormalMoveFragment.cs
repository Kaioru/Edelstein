using System;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments
{
    public class NormalMoveFragment : ActionMoveFragment
    {
        private Point2D _position;
        private Point2D _vPosition;
        private short _footholdID;
        private short _fallStartFootholdID;
        private Point2D _offset;

        public NormalMoveFragment(
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

        public override void ReadData(IPacketReader reader)
        {
            _position = reader.ReadPoint2D();
            _vPosition = reader.ReadPoint2D();
            _footholdID = reader.ReadShort();
            if (Attribute == MoveFragmentAttribute.FallDown)
                _fallStartFootholdID = reader.ReadShort();
            _offset = reader.ReadPoint2D();

            base.ReadData(reader);
        }

        public override void WriteData(IPacketWriter writer)
        {
            writer.WritePoint2D(_position);
            writer.WritePoint2D(_vPosition);
            writer.WriteShort(_footholdID);
            if (Attribute == MoveFragmentAttribute.FallDown)
                writer.WriteShort(_fallStartFootholdID);
            writer.WritePoint2D(_offset);

            base.WriteData(writer);
        }
    }
}
