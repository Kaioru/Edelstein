using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskYesNoRequest : AbstractConversationRequest<bool>
    {
        public override ConversationRequestType Type => ConversationRequestType.AskYesNo;
        private readonly string _text;

        public AskYesNoRequest(
            IConversationSpeaker speaker,
            string text
        ) : base(speaker)
        {
            _text = text;
        }

        public override bool Validate(IConversationResponse<bool> response)
            => true;

        public override void EncodeData(IPacketEncoder packet)
        {
            packet.EncodeString(_text);
        }
    }
}