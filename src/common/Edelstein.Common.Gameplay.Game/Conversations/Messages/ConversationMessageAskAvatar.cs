using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAskAvatar(
    IConversationSpeaker Speaker,
    string Text,
    int[] Styles
) : IConversationMessage<byte>
{
    public ConversationMessageType Type => ConversationMessageType.AskAvatar;

    public bool Check(byte answer)
        => answer < Styles.Length;
}
