using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class JumpPathFragment<TMoveAction> : ActionPathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private short _fallStartFootholdID;
    private IPoint2D _vPosition;

    public JumpPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _vPosition = reader.ReadPoint2D();

        if (Type is MovePathFragmentType.MobToss or MovePathFragmentType.MobTossSlowdown)
            _fallStartFootholdID = reader.ReadShort();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_vPosition);

        if (Type is MovePathFragmentType.MobToss or MovePathFragmentType.MobTossSlowdown)
            writer.WriteShort(_fallStartFootholdID);

        base.WriteBody(writer);
    }
}
