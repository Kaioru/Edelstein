using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerNPC(
    IConversationContext context,
    IFieldNPC npc,
    ConversationSpeakerParam @params = 0
) : ConversationSpeaker(context, ConversationSpeakerType.NPC, npc.Template.ID, @params),
    IConversationSpeakerNPC
{
    public IFieldNPC NPC => npc;
}
