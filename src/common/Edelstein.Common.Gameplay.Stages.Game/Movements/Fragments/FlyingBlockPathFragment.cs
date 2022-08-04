using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class FlyingBlockPathFragment : ActionPathFragment
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

    public override void Apply(MovePath path) => path.Position = _position;
}
