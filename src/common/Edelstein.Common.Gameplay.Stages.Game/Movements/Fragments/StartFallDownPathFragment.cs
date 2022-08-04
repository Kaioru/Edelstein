using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class StartFallDownPathFragment : ActionPathFragment
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
