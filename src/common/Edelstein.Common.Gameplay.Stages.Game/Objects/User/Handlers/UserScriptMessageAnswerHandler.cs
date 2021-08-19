using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserScriptMessageAnswerHandler : AbstractUserPacketHandler
    {

        public override short Operation => (short)PacketRecvOperations.UserScriptMessageAnswer;

        protected override async Task<bool> Check(GameStageUser stageUser, IFieldObjUser user)
            => await base.Check(stageUser, user) && user.IsConversing;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var type = (ConversationRequestType)packet.ReadByte();

            if (type == ConversationRequestType.AskQuiz ||
                type == ConversationRequestType.AskSpeedQuiz)
            {
                await user.ConversationContext.Respond(
                    new ConversationResponse<string>(type, packet.ReadString())
                );
                return;
            }

            var answer = packet.ReadByte();

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
                        new ConversationResponse<string>(type, packet.ReadString())
                    );
                    break;
                case ConversationRequestType.AskNumber:
                case ConversationRequestType.AskMenu:
                case ConversationRequestType.AskSlideMenu:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<int>(type, packet.ReadInt())
                    );
                    break;
                case ConversationRequestType.AskAvatar:
                case ConversationRequestType.AskMemberShopAvatar:
                    await user.ConversationContext.Respond(
                        new ConversationResponse<byte>(type, packet.ReadByte())
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