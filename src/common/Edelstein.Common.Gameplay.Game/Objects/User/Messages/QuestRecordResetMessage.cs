using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record QuestRecordResetMessage(
    int QuestID
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.QuestRecordMessage);
        writer.WriteShort((short)QuestID);
        writer.WriteByte(0);
        writer.WriteBool(false);
    }
}
