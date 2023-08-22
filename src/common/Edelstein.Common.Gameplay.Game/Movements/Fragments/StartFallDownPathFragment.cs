using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Movements.Fragments;

public class StartFallDownPathFragment<TMoveAction> : ActionPathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private short _fallStartFootholdID;
    private IPoint2D _vPosition;

    public StartFallDownPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _vPosition = reader.ReadPoint2D();
        _fallStartFootholdID = reader.ReadShort();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_vPosition);
        writer.WriteShort(_fallStartFootholdID);

        base.WriteBody(writer);
    }
}
