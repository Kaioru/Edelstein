using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Speakers;

public class ConversationSpeakerUser(
    IConversationContext context,
    IFieldUser user,
    ConversationSpeakerParam @params = 0
) : ConversationSpeaker(context, @params: @params),
    IConversationSpeakerUser
{
    public IFieldUser User => user;
}
