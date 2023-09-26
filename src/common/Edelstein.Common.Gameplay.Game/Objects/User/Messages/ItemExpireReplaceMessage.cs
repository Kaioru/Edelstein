using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record ItemExpireReplaceMessage(
    ICollection<string> Messages
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.ItemExpireReplaceMessage);
        writer.WriteByte((byte)Messages.Count);
        foreach (var s in Messages)
            writer.WriteString(s);
    }
}
