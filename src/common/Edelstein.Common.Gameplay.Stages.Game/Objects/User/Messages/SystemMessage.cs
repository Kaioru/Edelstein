using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public class SystemMessage : AbstractFieldMessage
{
    private readonly string _message;

    public SystemMessage(string message) => _message = message;

    protected override FieldMessageType Type => FieldMessageType.SystemMessage;

    protected override void WriteData(IPacketWriter writer) => writer.WriteString(_message);
}
