namespace Edelstein.Service.Game.Conversations.Messages
{
    public class AskMemberShopAvatarMessage : AskAvatarMessage
    {
        public override ConversationMessageType Type => ConversationMessageType.AskMemberShopAvatar;

        public AskMemberShopAvatarMessage(
            ISpeaker speaker,
            string text,
            int[] styles
        ) : base(speaker, text, styles)
        {
        }
    }
}