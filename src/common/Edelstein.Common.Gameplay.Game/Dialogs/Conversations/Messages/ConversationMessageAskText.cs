using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

public record ConversationMessageAskText(
    IConversationSpeaker Speaker,
    string Text,
    string Default,
    short LengthMin,
    short LengthMax
) : IConversationMessage<string>
{
    public ConversationMessageType Type => ConversationMessageType.AskText;

    public bool Check(string answer)
        => answer.Length >= LengthMin && answer.Length <= LengthMax;
}
