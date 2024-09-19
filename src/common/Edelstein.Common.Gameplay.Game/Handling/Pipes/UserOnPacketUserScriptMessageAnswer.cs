using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserScriptMessageAnswer : AbstractUserOnPacketInFieldPipe<UserScriptMessageAnswer>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserScriptMessageAnswer> message)
    {
        if (message.User.FieldSplit == null) return;
        if (message.User.ActiveConversation == null) return;

        var type = message.Packet.Type;
        var conversation = message.User.ActiveConversation;

        if (message.Packet.Info is StructuredScriptMessageAnswerInfoStatus status)
        {
            if (
                type != ConversationMessageType.Say &&
                type != ConversationMessageType.AskYesNo &&
                type != ConversationMessageType.AskAccept &&
                status.Status == byte.MinValue ||
                type is
                    ConversationMessageType.Say or
                    ConversationMessageType.AskYesNo or
                    ConversationMessageType.AskAccept &&
                status.Status == byte.MaxValue
            )
            {
                await message.User.EndConversation();
                return;
            }
        }

        switch (message.Packet.Info)
        {
            case StructuredScriptMessageAnswerInfoAnswer<LPString> answerString:
                await conversation.Answer(new ConversationMessageAnswer<string>(type, answerString.Answer.Value));
                break;
            case StructuredScriptMessageAnswerInfoAnswer<int> answerInt:
                await conversation.Answer(new ConversationMessageAnswer<int>(type, answerInt.Answer));
                break;
            case StructuredScriptMessageAnswerInfoAnswer<byte> answerByte:
                await conversation.Answer(new ConversationMessageAnswer<byte>(type, answerByte.Answer));
                break;
            case StructuredScriptMessageAnswerInfoQuiz answerQuiz:
                await conversation.Answer(new ConversationMessageAnswer<string>(type, answerQuiz.Answer.Value));
                break;
            case StructuredScriptMessageAnswerInfoStatus answerStatus:
                await conversation.Answer(new ConversationMessageAnswer<byte>(type, answerStatus.Status));
                break;
        }
    }
}
