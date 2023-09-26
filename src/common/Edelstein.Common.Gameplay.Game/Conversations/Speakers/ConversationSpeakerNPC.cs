using Edelstein.Common.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Conversations.Speakers;

public class ConversationSpeakerNPC : ConversationSpeaker, IConversationSpeakerNPC
{
    private readonly IFieldNPC _npc;

    public ConversationSpeakerNPC(
        IFieldNPC npc,
        IConversationContext context,
        int id = 9010000,
        ConversationSpeakerFlags flags = 0
    ) : base(context, id, flags)
        => _npc = npc;
    
    public ISpeakerField? Field => _npc.Field == null ? null : new SpeakerField(_npc.Field);
}
