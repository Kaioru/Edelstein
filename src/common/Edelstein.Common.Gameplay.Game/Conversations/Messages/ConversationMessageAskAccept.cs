using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAskAccept(
    IConversationSpeaker Speaker,
    string Text
) : IConversationMessage<byte>
{
    public ConversationMessageType Type => ConversationMessageType.AskAccept;

    public bool Check(byte answer) => true;
}
