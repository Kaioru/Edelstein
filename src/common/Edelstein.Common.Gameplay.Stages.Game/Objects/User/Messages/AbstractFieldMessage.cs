using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public abstract class AbstractFieldMessage : IFieldMessage
{
    public abstract FieldMessageType Type { get; }
    public abstract void WriteTo(IPacketWriter writer);
}
