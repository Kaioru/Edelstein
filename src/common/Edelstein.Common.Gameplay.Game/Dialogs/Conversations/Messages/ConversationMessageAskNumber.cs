using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

public record ConversationMessageAskNumber(
    IConversationSpeaker Speaker,
    string Text,
    int Default,
    int Min,
    int Max
) : IConversationMessage<int>
{
    public ConversationMessageType Type => ConversationMessageType.AskNumber;

    public bool Check(int answer)
        => answer >= Min && answer <= Max;
}
