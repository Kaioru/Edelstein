using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskMemberShopAvatarRequest : AskAvatarRequest
{
    public AskMemberShopAvatarRequest(
        IConversationSpeaker speaker,
        string text, int[] styles
    ) : base(speaker, text, styles)
    {
    }

    public override ConversationMessageType Type => ConversationMessageType.AskMemberShopAvatar;
}
