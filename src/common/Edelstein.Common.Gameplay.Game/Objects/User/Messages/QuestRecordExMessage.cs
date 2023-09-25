using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record QuestRecordExMessage(
    int QuestID,
    string Value
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.QuestRecordExMessage);
        writer.WriteShort((short)QuestID);
        writer.WriteString(Value);
    }
}
