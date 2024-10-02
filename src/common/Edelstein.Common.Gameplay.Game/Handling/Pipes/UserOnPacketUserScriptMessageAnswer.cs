using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserScriptMessageAnswer : AbstractUserOnPacketInField<UserScriptMessageAnswer>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserScriptMessageAnswer> message)
    {
        if (message.User.FieldSplit == null) return;
        if (message.User.ActiveDialog is not IConversationDialog conversation) return;

        var type = message.Packet.Type;
        var action = message.Packet.Action;
        var context = conversation.Context;
        
        if (
            type != ConversationMessageType.Say &&
            type != ConversationMessageType.SayImage &&
            type != ConversationMessageType.AskYesNo &&
            type != ConversationMessageType.AskAccept &&
            action == byte.MinValue ||
            type is
                ConversationMessageType.Say or
                ConversationMessageType.SayImage or
                ConversationMessageType.AskYesNo or
                ConversationMessageType.AskAccept &&
            action == byte.MaxValue
        )
        {
            await message.User.EndDialog();
            return;
        }

        switch (message.Packet.Info)
        {
            case StructuredScriptMessageAnswerInfoAnswer<LPString> answerString:
                if (action == 0)
                {
                    await message.User.EndDialog();
                    return;
                }
                
                await context.Answer(new ConversationMessageAnswer<string>(type, answerString.Answer.Value));
                break;
            case StructuredScriptMessageAnswerInfoAnswer<int> answerInt:
                if (action == 0)
                {
                    await message.User.EndDialog();
                    return;
                }

                await context.Answer(new ConversationMessageAnswer<int>(type, answerInt.Answer));
                break;
            case StructuredScriptMessageAnswerInfoAnswer<byte> answerByte:
                if (action == 0)
                {
                    await message.User.EndDialog();
                    return;
                }

                await context.Answer(new ConversationMessageAnswer<byte>(type, answerByte.Answer));
                break;
            default:
                await context.Answer(new ConversationMessageAnswer<byte>(type, action));
                break;
        }
    }
}
