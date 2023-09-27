using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects.Field;

public record TrembleFieldEffect(
    bool IsHeavyAndShort,
    int Delay
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)FieldEffectType.Tremble);
        writer.WriteBool(IsHeavyAndShort);
        writer.WriteInt(Delay);
    }
}
