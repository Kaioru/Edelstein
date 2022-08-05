using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public class SystemMessage : AbstractFieldMessage
{
    private readonly string _message;

    public SystemMessage(string message) => _message = message;

    public override FieldMessageType Type => FieldMessageType.SystemMessage;

    public override void WriteTo(IPacketWriter writer) => writer.WriteString(_message);
}
