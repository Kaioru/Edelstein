using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Messages.Types
{
    public class SystemMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.SystemMessage;
        private readonly string _text;

        public SystemMessage(string text)
        {
            _text = text;
        }

        public override void Encode(IPacket packet)
        {
            packet.Encode<string>(_text);
        }
    }
}