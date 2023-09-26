using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record IncSPMessage(
    short Job,
    byte SP
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.IncSPMessage);
        writer.WriteShort(Job);
        writer.WriteByte(SP);
    }
}
