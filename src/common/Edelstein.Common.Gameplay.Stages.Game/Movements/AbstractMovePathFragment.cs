using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements;

public abstract class AbstractMovePathFragment<TMoveAction> : IPacketReadable, IPacketWritable
    where TMoveAction : IMoveAction
{
    protected AbstractMovePathFragment(MovePathFragmentType type) => Type = type;

    protected MovePathFragmentType Type { get; }

    public void ReadFrom(IPacketReader reader) => ReadBody(reader);

    public void WriteTo(IPacketWriter writer)
    {
        WriteHeader(writer);
        WriteBody(writer);
    }

    private void WriteHeader(IPacketWriter writer) => writer.WriteByte((byte)Type);

    protected abstract void ReadBody(IPacketReader reader);
    protected abstract void WriteBody(IPacketWriter writer);

    public abstract void Apply(AbstractMovePath<TMoveAction> path);
}
