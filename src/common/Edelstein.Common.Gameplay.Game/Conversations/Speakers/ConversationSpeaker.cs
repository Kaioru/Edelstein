using Edelstein.Common.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeaker(
    IConversationContext context,
    ConversationSpeakerType type = ConversationSpeakerType.User, 
    int templateID = 9010000,
    ConversationSpeakerParam @params = 0
) : IConversationSpeaker
{
    public ConversationSpeakerType Type { get; } = type;
    public int TemplateID { get; } = templateID;
    public ConversationSpeakerParam Params { get; } = @params;

    public byte Say(string text, bool prev = false, bool next = true)
        => context.Ask(new ConversationMessageSay(
            this,
            text,
            prev,
            next
        )).Result;
}
