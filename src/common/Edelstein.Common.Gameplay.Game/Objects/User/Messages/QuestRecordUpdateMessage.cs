using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record QuestRecordUpdateMessage(
    int QuestID,
    string Record
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.QuestRecordMessage);
        writer.WriteUShort((ushort)QuestID);
        writer.WriteByte(1);
        writer.WriteString(Record);
    }
}
