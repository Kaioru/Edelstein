using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAskBoxText(
    IConversationSpeaker Speaker,
    string Text,
    string Default,
    short Col,
    short Row
) : IConversationMessage<string>
{
    public ConversationMessageType Type => ConversationMessageType.AskBoxText;

    public bool Check(string answer) => true;
}
