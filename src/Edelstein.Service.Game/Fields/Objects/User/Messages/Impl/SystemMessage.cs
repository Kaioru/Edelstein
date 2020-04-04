using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages.Impl
{
    public class SystemMessage : AbstractMessage
    {
        public override MessageType Type => MessageType.SystemMessage;
        private readonly string _text;

        public SystemMessage(string text)
        {
            _text = text;
        }

        protected override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeString(_text);
        }
    }
}