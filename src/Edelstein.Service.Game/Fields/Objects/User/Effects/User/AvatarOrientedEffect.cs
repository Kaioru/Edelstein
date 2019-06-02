using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Effects.User
{
    public class AvatarOrientedEffect : AbstractEffect
    {
        public override EffectType Type => EffectType.AvatarOriented;

        private readonly string _path;

        public AvatarOrientedEffect(string path)
        {
            _path = path;
        }

        protected override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_path);
            packet.Encode<int>(0);
        }
    }
}