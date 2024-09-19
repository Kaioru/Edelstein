using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

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
