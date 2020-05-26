using Edelstein.Core.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskTextRequest : AbstractConversationRequest<string>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskText;
        private readonly string _text;
        private readonly string _def;
        private readonly short _lenMin;
        private readonly short _lenMax;

        public AskTextRequest(
            IConversationSpeaker speaker,
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

        public override bool Validate(IConversationResponse<string> response)
            => response.Value.Length >= _lenMin &&
               response.Value.Length <= _lenMax;

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeString(_text);
            packet.EncodeString(_def);
            packet.EncodeShort(_lenMin);
            packet.EncodeShort(_lenMax);
        }
    }
}