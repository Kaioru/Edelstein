using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class NormalPathFragment : ActionPathFragment
{
    private short _fallStartFootholdID;
    private short _footholdID;
    private IPoint2D _offset;
    private IPoint2D _position;
    private IPoint2D _vPosition;

    public NormalPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _position = reader.ReadPoint2D();
        _vPosition = reader.ReadPoint2D();
        _footholdID = reader.ReadShort();
        if (Type == MovePathFragmentType.FallDown)
            _fallStartFootholdID = reader.ReadShort();
        _offset = reader.ReadPoint2D();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_position);
        writer.WritePoint2D(_vPosition);
        writer.WriteShort(_footholdID);
        if (Type == MovePathFragmentType.FallDown)
            writer.WriteShort(_fallStartFootholdID);
        writer.WritePoint2D(_offset);

        base.WriteBody(writer);
    }

    public override void Apply(MovePath path)
    {
        path.Position = _position;
        path.FootholdID = _footholdID;
    }
}
