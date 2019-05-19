using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects
{
    public interface IEffect
    {
        EffectType Type { get; }
        void Encode(IPacket packet);
    }
}