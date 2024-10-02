using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

public interface IConversationSpeakerNPC : IConversationSpeaker
{
    IFieldNPC NPC { get; }
}
