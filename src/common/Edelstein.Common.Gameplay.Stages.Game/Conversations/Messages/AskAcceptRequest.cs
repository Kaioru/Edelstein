using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Speakers;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Messages;

public class AskAcceptRequest : AskYesNoRequest
{
    public AskAcceptRequest(IConversationSpeaker speaker, string text) : base(speaker, text)
    {
    }

    public override ConversationMessageType Type => ConversationMessageType.AskAccept;
}
