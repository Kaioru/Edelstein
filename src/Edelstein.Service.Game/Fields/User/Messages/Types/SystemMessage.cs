using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Fields.User.Messages.Types
{
    public class SystemMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.SystemMessage;
        private readonly string _text;

        public SystemMessage(string text)
            => _text = text;

        protected override void EncodeData(IPacket packet)
            => packet.Encode<string>(_text);
    }
}