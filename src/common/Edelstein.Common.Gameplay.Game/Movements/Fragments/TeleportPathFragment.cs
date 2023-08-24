using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Movements.Fragments;

public class TeleportPathFragment<TMoveAction> : ActionPathFragment<TMoveAction>
    where TMoveAction : IMoveAction
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

    public override void Apply(AbstractMovePath<TMoveAction> path)
    {
        path.Position = _position;
        path.FootholdID = _footholdID;

        base.Apply(path);
    }
}
