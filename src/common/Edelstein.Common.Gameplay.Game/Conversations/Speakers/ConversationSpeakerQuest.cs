using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerQuest : ConversationSpeaker, IConversationSpeakerQuest
{
    public ConversationSpeakerQuest(
        int questID,
        IConversationContext context,
        int id = 9010000,
        ConversationSpeakerFlags flags = 0
    ) : base(context, id, flags)
        => QuestID = questID;
    
    public int QuestID { get; }
}
