using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

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
