using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects;

public record QuestEffect(
    ICollection<Tuple<int, int>> Items
) : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte((byte)EffectType.Quest);
        writer.WriteByte((byte)Items.Count);
        foreach (var tuple in Items)
        {
            writer.WriteInt(tuple.Item1);
            writer.WriteInt(tuple.Item2);
        }
    }
}
