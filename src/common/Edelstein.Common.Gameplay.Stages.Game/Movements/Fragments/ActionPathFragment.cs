using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class ActionPathFragment<TMoveAction> : AbstractMovePathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private byte _action;
    private short _elapse;
    private bool _forcedStop;

    public ActionPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _action = reader.ReadByte();
        _elapse = reader.ReadShort();
        _forcedStop = reader.ReadBool();
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WriteByte(_action);
        writer.WriteShort(_elapse);
        writer.WriteBool(_forcedStop);
    }

    public override void Apply(AbstractMovePath<TMoveAction> path) =>
        path.ActionRaw = _action;
}
