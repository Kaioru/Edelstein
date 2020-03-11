using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class SayRequest : AbstractConversationRequest<byte>
    {
        public override ConversationRequestType Type => ConversationRequestType.Say;
        private readonly string _text;
        private readonly bool _prev;
        private readonly bool _next;

        public SayRequest(
            IConversationSpeaker speaker,
            string text,
            bool prev,
            bool next
        ) : base(speaker)
        {
            _text = text;
            _prev = prev;
            _next = next;
        }

        public override bool Validate(IConversationResponse<byte> response)
            => true;

        public override void EncodeData(IPacket packet)
        {
            if (Speaker.Type.HasFlag(ConversationSpeakerType.NPCReplacedByNPC))
                packet.Encode<int>(Speaker.TemplateID);
            packet.Encode<string>(_text);
            packet.Encode<bool>(_prev);
            packet.Encode<bool>(_next);
        }
    }
}