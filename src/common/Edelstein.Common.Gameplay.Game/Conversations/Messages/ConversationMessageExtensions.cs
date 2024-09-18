using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public static class ConversationMessageExtensions
{
    public static StructuredScriptMessage ToStructured<T>(this IConversationMessage<T> message)
        => new()
        {
            SpeakerType = message.Speaker.Type,
            SpeakerTemplateID = message.Speaker.TemplateID,
            Type = message.Type,
            Params = (byte)message.Speaker.Params,
            Info = message switch
            {
                ConversationMessageSay say => new StructuredScriptMessageInfoSay
                {
                    Text = new LPString(say.Text),
                    Next = say.Next,
                    Prev = say.Prev
                },
                _ => new StructuredScriptMessageInfo()
            }
        };
}
