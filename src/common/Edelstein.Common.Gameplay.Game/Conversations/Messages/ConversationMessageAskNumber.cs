using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

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
