using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class StatChangeFragment : AbstractMovePathFragment
{
    private bool _stat;

    public StatChangeFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader) =>
        _stat = reader.ReadBool();

    protected override void WriteBody(IPacketWriter writer) =>
        writer.WriteBool(_stat);

    public override void Apply(MovePath path)
    {
    }
}
