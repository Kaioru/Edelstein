using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects
{
    public interface IFieldEffect
    {
        FieldEffectType Type { get; }
        void Encode(IPacket packet);
    }
}