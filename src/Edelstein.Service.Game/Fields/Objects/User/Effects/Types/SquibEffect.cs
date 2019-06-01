using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects.Types
{
    public class SquibEffect : AbstractEffect
    {
        public override EffectType Type => EffectType.SquibEffect;

        private readonly string _path;

        public SquibEffect(string path)
        {
            _path = path;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_path);
        }
    }
}