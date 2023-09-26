using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects;

public record AvatarOrientedEffect(
    string Path
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)EffectType.AvatarOriented);
        writer.WriteString(Path);
        writer.WriteInt(0);
    }
}
