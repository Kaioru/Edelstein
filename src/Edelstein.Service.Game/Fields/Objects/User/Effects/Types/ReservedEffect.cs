using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects.Types
{
    public class ReservedEffect : AbstractEffect
    {
        public override EffectType Type => EffectType.ReservedEffect;

        private readonly string _path;

        public ReservedEffect(string path)
        {
            _path = path;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_path);
        }
    }
}