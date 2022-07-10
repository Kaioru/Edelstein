using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Requests
{
    public class AskMemberShopAvatarConversationRequest : AskAvatarConversationRequest
    {
        public override ConversationRequestType Type => ConversationRequestType.AskMemberShopAvatar;

        public AskMemberShopAvatarConversationRequest(
            IConversationSpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker, text, styles)
        {
        }
    }
}
