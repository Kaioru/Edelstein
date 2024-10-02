using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

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
