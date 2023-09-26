using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record ItemProtectExpireMessage(
    ICollection<int> ItemID
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.ItemProtectExpireMessage);
        writer.WriteByte((byte)ItemID.Count);
        foreach (var i in ItemID)
            writer.WriteInt(i);
    }
}
