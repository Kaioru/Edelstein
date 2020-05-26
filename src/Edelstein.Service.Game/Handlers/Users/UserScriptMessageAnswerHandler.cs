using System;
using System.Threading.Tasks;
using Baseline;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class UserScriptMessageAnswerHandler : AbstractFieldUserHandler
    {
        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            if (user.ConversationContext == null) return;

            var type = (ConversationRequestType) packet.DecodeByte();

            if (type == ConversationRequestType.AskQuiz ||
                type == ConversationRequestType.AskSpeedQuiz)
            {
                await user.ConversationContext.Respond(
                    new ConversationResponse<string>(type, packet.DecodeString())
                );
                return;
            }

            var answer = packet.DecodeByte();

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
                user.ConversationContext.SafeDispose();
                return;
            }

            switch (type)
            {
                case ConversationRequestType.AskText:
                case ConversationRequestType.AskBoxText:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<string>(type, packet.DecodeString())
                    );
                    break;
                case ConversationRequestType.AskNumber:
                case ConversationRequestType.AskMenu:
                case ConversationRequestType.AskSlideMenu:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<int>(type, packet.DecodeInt())
                    );
                    break;
                case ConversationRequestType.AskAvatar:
                case ConversationRequestType.AskMemberShopAvatar:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<byte>(type, packet.DecodeByte())
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