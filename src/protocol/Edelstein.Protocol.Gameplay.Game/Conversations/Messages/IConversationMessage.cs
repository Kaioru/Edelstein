using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public interface IConversationMessage<in T>
{
    ConversationMessageType Type { get; }
    IConversationSpeaker Speaker { get; }

    bool Check(T answer);
}
