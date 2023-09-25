using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record DropPickUpMoneyMessage(
    int Money,
    bool IsPortionMissing = false,
    short PremiumBonus = 0
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.DropPickUpMessage);
        writer.WriteByte(1);
        writer.WriteBool(IsPortionMissing);
        writer.WriteInt(Money);
        writer.WriteShort(PremiumBonus);
    }
}
