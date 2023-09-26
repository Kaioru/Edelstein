using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Messages;

public record SkillExpireMessage(
    ICollection<int> SkillID
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)MessageType.SkillExpireMessage);
        writer.WriteByte((byte)SkillID.Count);
        foreach (var s in SkillID)
            writer.WriteInt(s);
    }
}
