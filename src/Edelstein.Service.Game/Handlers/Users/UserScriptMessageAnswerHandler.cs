using System;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserScriptMessageAnswerHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            if (user.ConversationContext == null) return;

            var type = (ConversationRequestType) packet.Decode<byte>();

            if (type == ConversationRequestType.AskQuiz ||
                type == ConversationRequestType.AskSpeedQuiz)
            {
                await user.ConversationContext.Respond(
                    new ConversationResponse<string>(type, packet.Decode<string>())
                );
                return;
            }

            var answer = packet.Decode<byte>();

            if (
                type != ConversationRequestType.Say &&
                type != ConversationRequestType.AskYesNo &&
                type != ConversationRequestType.AskAccept &&
                answer == byte.MinValue ||
                (type == ConversationRequestType.Say ||
                 type == ConversationRequestType.AskYesNo ||
                 type == ConversationRequestType.AskAccept) && answer == byte.MaxValue
            )
            {
                user.ConversationContext.Dispose();
                return;
            }

            switch (type)
            {
                case ConversationRequestType.AskText:
                case ConversationRequestType.AskBoxText:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<string>(type, packet.Decode<string>())
                    );
                    break;
                case ConversationRequestType.AskNumber:
                case ConversationRequestType.AskMenu:
                case ConversationRequestType.AskSlideMenu:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<int>(type, packet.Decode<int>())
                    );
                    break;
                case ConversationRequestType.AskAvatar:
                case ConversationRequestType.AskMemberShopAvatar:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<byte>(type, packet.Decode<byte>())
                    );
                    break;
                case ConversationRequestType.AskYesNo:
                case ConversationRequestType.AskAccept:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<bool>(type, Convert.ToBoolean(answer))
                    );
                    break;
                default:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<byte>(type, answer)
                    );
                    break;
            }
        }
    }
}