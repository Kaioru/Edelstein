using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class AskAcceptRequest : AskYesNoRequest
{
    public override ConversationMessageType Type => ConversationMessageType.AskAccept;
    
    public AskAcceptRequest(IConversationSpeaker speaker, string text) : base(speaker, text)
    {
    }
}
