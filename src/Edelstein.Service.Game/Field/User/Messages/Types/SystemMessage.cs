using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Field.User.Messages.Types
{
    public class SystemMessage : AbstractMessage
    {
        public override MessageType Type { get; }
        private readonly string _text;

        public SystemMessage(string text)
            => _text = text;

        protected override void EncodeData(IPacket packet)
            => packet.Encode<string>(_text);
    }
}