using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerPortal(
    IConversationContext context,
    ConversationSpeakerParam @params = 0
) : ConversationSpeaker(context, @params: @params),
    IConversationSpeakerPortal;
