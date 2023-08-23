using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class AskMemberShopAvatarRequest : AskAvatarRequest
{
    public override ConversationMessageType Type => ConversationMessageType.AskMemberShopAvatar;
    
    public AskMemberShopAvatarRequest(
        IConversationSpeaker speaker,
        string text, int[] styles
    ) : base(speaker, text, styles)
    {
    }
}
