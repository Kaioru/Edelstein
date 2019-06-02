using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects
{
    public abstract class AbstractFieldEffect : IFieldEffect
    {
        public abstract FieldEffectType Type { get; }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            EncodeData(packet);
        }

        protected abstract void EncodeData(IPacket packet);
    }
}