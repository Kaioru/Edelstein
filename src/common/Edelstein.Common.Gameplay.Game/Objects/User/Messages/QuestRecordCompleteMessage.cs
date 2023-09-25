using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record QuestRecordCompleteMessage(
    int QuestID,
    DateTime DateFinish
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.QuestRecordMessage);
        writer.WriteShort((short)QuestID);
        writer.WriteByte(2);
        writer.WriteDateTime(DateFinish);
    }
}
