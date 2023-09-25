using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects;

public class JobChangedEffect : IPacketWritable
{
    public void WriteTo(IPacketWriter writer)
        => writer.WriteByte((byte)EffectType.JobChanged);
}
