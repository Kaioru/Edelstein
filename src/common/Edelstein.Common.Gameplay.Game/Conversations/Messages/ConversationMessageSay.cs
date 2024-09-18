using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageSay(
    IConversationSpeaker Speaker,
    string Text,
    bool Prev,
    bool Next
) : IConversationMessage<byte>
{
    public ConversationMessageType Type => ConversationMessageType.Say;

    public bool Check(byte answer) => true;
}
