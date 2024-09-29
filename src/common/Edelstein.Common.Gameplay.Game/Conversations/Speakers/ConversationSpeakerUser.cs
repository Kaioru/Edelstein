using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerUser(
    IConversationContext context,
    IFieldUser user,
    ConversationSpeakerParam @params = 0
) : ConversationSpeaker(context, @params: @params),
    IConversationSpeakerUser
{
    public IFieldUser User => user;
}
