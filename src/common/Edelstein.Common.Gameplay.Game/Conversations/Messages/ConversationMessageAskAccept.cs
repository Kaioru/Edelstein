using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAskAccept(
    IConversationSpeaker Speaker,
    string Text
) : IConversationMessage<bool>
{
    public ConversationMessageType Type => ConversationMessageType.AskAccept;

    public bool Check(bool answer) => true;
}
