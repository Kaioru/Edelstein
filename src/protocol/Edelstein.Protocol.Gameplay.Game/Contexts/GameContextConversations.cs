using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextConversations(
    IConversationManager<IConversationSpeakerNPC, IConversationSpeakerUser> NPC,
    IConversationManager<IConversationSpeakerPortal, IConversationSpeakerUser> Portal
);
