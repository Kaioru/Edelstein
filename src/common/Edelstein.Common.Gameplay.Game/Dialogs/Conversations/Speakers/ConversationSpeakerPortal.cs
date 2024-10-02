using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Speakers;

public class ConversationSpeakerPortal(
    IConversationContext context,
    ConversationSpeakerParam @params = 0
) : ConversationSpeaker(context, @params: @params),
    IConversationSpeakerPortal;
