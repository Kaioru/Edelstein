﻿using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record QuestRecordAcceptMessage(
    int QuestID,
    string Record
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.QuestRecordMessage);
        writer.WriteShort((short)QuestID);
        writer.WriteByte(1);
        writer.WriteString(Record);
    }
}
