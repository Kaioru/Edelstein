using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextConversations(
    IConversationManager<IConversationSpeakerNPC, IConversationSpeakerUser> NPC,
    IConversationManager<IConversationSpeakerPortal, IConversationSpeakerUser> Portal
);
