using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Effects
{
    public abstract class AbstractEffect : IEffect
    {
        public abstract EffectType Type { get; }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            EncodeData(packet);
        }

        protected abstract void EncodeData(IPacket packet);
    }
}