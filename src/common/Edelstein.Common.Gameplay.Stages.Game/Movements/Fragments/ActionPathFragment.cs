using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class ActionPathFragment : AbstractMovePathFragment
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

    public override void Apply(MovePath path)
    {
    }
}
