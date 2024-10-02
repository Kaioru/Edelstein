using Edelstein.Protocol.Gameplay.Game.Objects.Users;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

public interface IConversationSpeakerUser : IConversationSpeaker
{
    IFieldUser User { get; }
}
