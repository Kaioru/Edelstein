using Edelstein.Common.Gameplay.Game.Conversations.Messages;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserScriptMessageAnswerHandler : AbstractFieldHandler
{
    public override short Operation => (short)PacketRecvOperations.UserScriptMessageAnswer;

    protected override async Task Handle(IFieldUser user, IPacketReader reader)
    {
        if (!user.IsConversing) return;
        if (user.ActiveConversation == null) return;

        var type = (ConversationMessageType)reader.ReadByte();

        if (type is ConversationMessageType.AskQuiz or ConversationMessageType.AskSpeedQuiz)
        {
            await user.ActiveConversation.Respond(
                new ConversationMessageResponse<string>(type, reader.ReadString())
            );
            return;
        }

        var answer = reader.ReadByte();

        if (
            (
                type != ConversationMessageType.Say &&
                type != ConversationMessageType.AskYesNo &&
                type != ConversationMessageType.AskAccept &&
                answer == byte.MinValue
            ) ||
            (
                type is
                    ConversationMessageType.Say or
                    ConversationMessageType.AskYesNo or
                    ConversationMessageType.AskAccept &&
                answer == byte.MaxValue
            )
        )
        {
            await user.EndConversation();
            return;
        }

        switch (type)
        {
            case ConversationMessageType.AskText:
            case ConversationMessageType.AskBoxText:
                await user.ActiveConversation.Respond(
                    new ConversationMessageResponse<string>(type, reader.ReadString())
                );
                break;
            case ConversationMessageType.AskNumber:
            case ConversationMessageType.AskMenu:
            case ConversationMessageType.AskSlideMenu:
                await user.ActiveConversation.Respond(
                    new ConversationMessageResponse<int>(type, reader.ReadInt())
                );
                break;
            case ConversationMessageType.AskAvatar:
            case ConversationMessageType.AskMemberShopAvatar:
                await user.ActiveConversation.Respond(
                    new ConversationMessageResponse<byte>(type, reader.ReadByte())
                );
                break;
            case ConversationMessageType.AskYesNo:
            case ConversationMessageType.AskAccept:
                await user.ActiveConversation.Respond(
                    new ConversationMessageResponse<bool>(type, Convert.ToBoolean(answer))
                );
                break;
            default:
                await user.ActiveConversation.Respond(
                    new ConversationMessageResponse<byte>(type, answer)
                );
                break;
        }
    }
}
