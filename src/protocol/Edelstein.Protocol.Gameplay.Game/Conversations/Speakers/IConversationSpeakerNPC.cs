using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers.Facades;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

public interface IConversationSpeakerNPC : IConversationSpeaker
{
    ISpeakerField? Field { get; }
}
