using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Movements.Fragments;

public class FlyingBlockPathFragment<TMoveAction> : ActionPathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private IPoint2D _position;
    private IPoint2D _vPosition;

    public FlyingBlockPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _position = reader.ReadPoint2D();
        _vPosition = reader.ReadPoint2D();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_position);
        writer.WritePoint2D(_vPosition);

        base.WriteBody(writer);
    }

    public override void Apply(AbstractMovePath<TMoveAction> path)
    {
        path.Position = _position;

        base.Apply(path);
    }
}
