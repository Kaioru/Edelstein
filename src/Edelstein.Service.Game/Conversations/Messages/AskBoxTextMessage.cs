using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskBoxTextMessage : AbstractConversationMessage<string>
    {
        public override ConversationMessageType Type => ConversationMessageType.AskBoxText;
        private readonly string _text;
        private readonly string _def;
        private readonly short _cols;
        private readonly short _rows;

        public AskBoxTextMessage(
            ISpeaker speaker,
            string text,
            string def,
            short cols,
            short rows
        ) : base(speaker)
        {
            _text = text;
            _def = def;
            _cols = cols;
            _rows = rows;
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<string>(_def);
            packet.Encode<short>(_cols);
            packet.Encode<short>(_rows);
        }

        public override bool Validate(string response)
            => true;
    }
}