using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record DropPickUpItemMessage(
    int ItemID,
    int Quantity
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.DropPickUpMessage);
        writer.WriteByte((byte)(Quantity == 1 ? 2 : 0));
        writer.WriteInt(ItemID);
        if (Quantity != 1)
            writer.WriteInt(Quantity);
    }
}
