using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.User.Effects;

public record PlayPortalSEEffect : IPacketWritable
{
    public void WriteTo(IPacketWriter writer) => writer.WriteByte((byte)EffectType.PlayPortalSE);
}
