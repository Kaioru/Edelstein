using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextConversations(
    IConversationManager<IConversationSpeaker, IConversationSpeaker> NPC
);
