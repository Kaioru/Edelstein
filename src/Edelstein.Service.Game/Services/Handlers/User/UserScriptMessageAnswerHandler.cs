using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers.User
{
    public class UserScriptMessageAnswerHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            if (user.ConversationContext == null) return;

            var type = (ConversationMessageType) packet.Decode<byte>();

            if (type != user.ConversationContext.LastRequestType) return;
            if (type == ConversationMessageType.AskQuiz ||
                type == ConversationMessageType.AskSpeedQuiz)
            {
                await user.ConversationContext.Respond(packet.Decode<string>());
                return;
            }

            var answer = packet.Decode<byte>();

            if (
                type != ConversationMessageType.Say &&
                type != ConversationMessageType.AskYesNo &&
                type != ConversationMessageType.AskAccept &&
                answer == byte.MinValue ||
                (type == ConversationMessageType.Say ||
                 type == ConversationMessageType.AskYesNo ||
                 type == ConversationMessageType.AskAccept) && answer == byte.MaxValue
            )
            {
                user.ConversationContext.TokenSource.Cancel();
                return;
            }

            switch (type)
            {
                case ConversationMessageType.AskText:
                case ConversationMessageType.AskBoxText:
                    await user.ConversationContext.Respond(packet.Decode<string>());
                    break;
                case ConversationMessageType.AskNumber:
                case ConversationMessageType.AskMenu:
                case ConversationMessageType.AskSlideMenu:
                    await user.ConversationContext.Respond(packet.Decode<int>());
                    break;
                case ConversationMessageType.AskAvatar:
                case ConversationMessageType.AskMemberShopAvatar:
                    await user.ConversationContext.Respond(packet.Decode<byte>());
                    break;
                case ConversationMessageType.AskYesNo:
                case ConversationMessageType.AskAccept:
                    await user.ConversationContext.Respond(Convert.ToBoolean(answer));
                    break;
                default:
                    await user.ConversationContext.Respond(answer);
                    break;
            }
        }
    }
}