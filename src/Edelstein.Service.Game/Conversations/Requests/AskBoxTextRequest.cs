using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskBoxTextRequest : AbstractConversationRequest<string>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskBoxText;
        private readonly string _text;
        private readonly string _def;
        private readonly short _cols;
        private readonly short _rows;

        public AskBoxTextRequest(
            IConversationSpeaker speaker,
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

        public override bool Validate(IConversationResponse<string> response)
            => true;

        public override void EncodeData(IPacket packet)
        {
            packet.EncodeString(_text);
            packet.EncodeString(_def);
            packet.EncodeShort(_cols);
            packet.EncodeShort(_rows);
        }
    }
}