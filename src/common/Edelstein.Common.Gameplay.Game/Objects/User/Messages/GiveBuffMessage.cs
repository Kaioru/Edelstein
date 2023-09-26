using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record GiveBuffMessage(
    int ItemID
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.GiveBuffMessage);
        writer.WriteInt(ItemID);
    }
}
