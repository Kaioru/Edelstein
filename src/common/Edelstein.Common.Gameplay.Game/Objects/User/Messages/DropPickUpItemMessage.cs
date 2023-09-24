using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record DropPickUpItemMessage(
    int ItemID,
    int Quantity,
    bool IsInChat
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.DropPickUpMessage);
        writer.WriteByte((byte)(IsInChat ? 2 : 0));
        writer.WriteInt(ItemID);
        writer.WriteInt(Quantity);
    }
}
