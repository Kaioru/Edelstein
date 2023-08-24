using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Movements.Fragments;

public class ActionPathFragment<TMoveAction> : AbstractMovePathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private byte _action;
    private short _elapse;

    public ActionPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _action = reader.ReadByte();
        _elapse = reader.ReadShort();
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WriteByte(_action);
        writer.WriteShort(_elapse);
    }

    public override void Apply(AbstractMovePath<TMoveAction> path) =>
        path.ActionRaw = _action;
}
