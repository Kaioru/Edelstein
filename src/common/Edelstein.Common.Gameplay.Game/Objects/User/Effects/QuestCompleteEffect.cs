using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects;

public record QuestCompleteEffect : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
        => writer.WriteByte((byte)EffectType.QuestComplete);
}
