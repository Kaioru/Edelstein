using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects;

public record ReservedEffect(
    string Path
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)EffectType.ReservedEffect);
        writer.WriteString(Path);
    }
}
