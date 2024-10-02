using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

public record ConversationMessageAskYesNo(
    IConversationSpeaker Speaker,
    string Text
) : IConversationMessage<byte>
{
    public ConversationMessageType Type => ConversationMessageType.AskYesNo;

    public bool Check(byte answer) => true;
}
