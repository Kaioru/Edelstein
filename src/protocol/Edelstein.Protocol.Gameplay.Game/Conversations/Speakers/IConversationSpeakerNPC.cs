using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeakerNPC : IConversationSpeaker
{
    IFieldNPC NPC { get; }
}
