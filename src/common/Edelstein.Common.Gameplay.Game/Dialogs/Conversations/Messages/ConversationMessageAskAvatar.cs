using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

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
