using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskAcceptConvesationRequest : AskYesNoConversationRequest
    {
        public override ConversationRequestType Type => ConversationRequestType.AskAccept;

        public AskAcceptConvesationRequest(IConversationSpeaker speaker, string text) : base(speaker, text)
        {
        }
    }
}
