using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects
{
    public class Effect : AbstractEffect
    {
        public override EffectType Type { get; }

        public Effect(EffectType type)
        {
            Type = type;
        }

        protected override void EncodeData(IPacket packet)
        {
        }
    }
}