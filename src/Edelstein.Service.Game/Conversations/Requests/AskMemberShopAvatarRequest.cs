using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations.Requests
{
    public class AskMemberShopAvatarRequest : AskAvatarRequest
    {
        public override ConversationRequestType Type => ConversationRequestType.AskMemberShopAvatar;

        public AskMemberShopAvatarRequest(
            IConversationSpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker, text, styles)
        {
        }
    }
}