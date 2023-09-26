using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record CashItemExpireMessage(
    int ItemID
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.CashItemExpireMessage);
        writer.WriteInt(ItemID);
    }
}
