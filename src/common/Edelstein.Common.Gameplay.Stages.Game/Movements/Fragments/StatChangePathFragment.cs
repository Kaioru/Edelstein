using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class StatChangePathFragment<TMoveAction> : AbstractMovePathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private bool _stat;

    public StatChangePathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader) =>
        _stat = reader.ReadBool();

    protected override void WriteBody(IPacketWriter writer) =>
        writer.WriteBool(_stat);

    public override void Apply(AbstractMovePath<TMoveAction> path)
    {
    }
}
