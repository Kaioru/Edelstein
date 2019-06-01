using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects.Field
{
    public class ScreenFieldEffect : AbstractFieldEffect
    {
        public override FieldEffectType Type => FieldEffectType.Screen;

        private readonly string _path;

        public ScreenFieldEffect(string path)
        {
            _path = path;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_path);
        }
    }
}