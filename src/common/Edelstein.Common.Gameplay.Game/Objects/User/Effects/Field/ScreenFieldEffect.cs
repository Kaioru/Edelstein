using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects.Field;

public record ScreenFieldEffect(
    string Path
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)FieldEffectType.Screen);
        writer.WriteString(Path);
    }
}
