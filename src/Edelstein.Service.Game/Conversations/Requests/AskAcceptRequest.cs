using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskAcceptRequest : AskYesNoRequest
    {
        public override ConversationRequestType Type => ConversationRequestType.AskAccept;

        public AskAcceptRequest(
            IConversationSpeaker speaker,
            string text
        ) : base(speaker, text)
        {
        }
    }
}