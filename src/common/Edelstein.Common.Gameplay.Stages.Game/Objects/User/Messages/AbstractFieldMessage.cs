using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public abstract class AbstractFieldMessage : IFieldMessage
{
    protected abstract FieldMessageType Type { get; }

    public void WriteTo(IPacketWriter writer)
    {
        WriteHeader(writer);
        WriteData(writer);
    }

    private void WriteHeader(IPacketWriter writer) => writer.WriteByte((byte)Type);
    protected abstract void WriteData(IPacketWriter writer);
}
