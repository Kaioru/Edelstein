using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageSayImage(
    IConversationSpeaker Speaker,
    string[] Path
) : IConversationMessage<byte>
{
    public ConversationMessageType Type => ConversationMessageType.SayImage;

    public bool Check(byte answer) => true;
}
