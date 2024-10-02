using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

public record ConversationMessageSayImage(
    IConversationSpeaker Speaker,
    string[] Path
) : IConversationMessage<byte>
{
    public ConversationMessageType Type => ConversationMessageType.SayImage;

    public bool Check(byte answer) => true;
}
