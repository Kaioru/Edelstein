using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskTextMessage : AbstractConversationMessage<string>
    {
        public override ConversationMessageType Type => ConversationMessageType.AskText;
        private readonly string _text;
        private readonly string _def;
        private readonly short _lenMin;
        private readonly short _lenMax;

        public AskTextMessage(
            ISpeaker speaker,
            string text,
            string def,
            short lenMin,
            short lenMax
        ) : base(speaker)
        {
            _text = text;
            _def = def;
            _lenMin = lenMin;
            _lenMax = lenMax;
        }

        public override void EncodeData(IPacket packet)
        {
            packet.Encode<string>(_text);
            packet.Encode<string>(_def);
            packet.Encode<short>(_lenMin);
            packet.Encode<short>(_lenMax);
        }

        public override bool Validate(string response)
            => response.Length >= _lenMin &&
               response.Length <= _lenMax;
    }
}