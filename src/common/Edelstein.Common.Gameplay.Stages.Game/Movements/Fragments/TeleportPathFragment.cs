using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class TeleportPathFragment : ActionPathFragment
{
    private short _footholdID;
    private IPoint2D _position;

    public TeleportPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _position = reader.ReadPoint2D();
        _footholdID = reader.ReadShort();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_position);
        writer.WriteShort(_footholdID);

        base.WriteBody(writer);
    }

    public override void Apply(MovePath path)
    {
        path.Position = _position;
        path.Foothold = _footholdID;
    }
}
