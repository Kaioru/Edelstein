using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class NormalPathFragment<TMoveAction> : ActionPathFragment<TMoveAction>
    where TMoveAction : IMoveAction
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
        if (Type is MovePathFragmentType.FallDown or MovePathFragmentType.DragDown)
            _fallStartFootholdID = reader.ReadShort();
        _offset = reader.ReadPoint2D();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_position);
        writer.WritePoint2D(_vPosition);
        writer.WriteShort(_footholdID);
        if (Type is MovePathFragmentType.FallDown or MovePathFragmentType.DragDown)
            writer.WriteShort(_fallStartFootholdID);
        writer.WritePoint2D(_offset);

        base.WriteBody(writer);
    }

    public override void Apply(AbstractMovePath<TMoveAction> path)
    {
        path.Position = _position;
        path.Foothold = _footholdID;

        base.Apply(path);
    }
}
