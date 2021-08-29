using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserEmotionHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserEmotion;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var emotion = packet.ReadInt();
            var duration = packet.ReadInt();
            var byItemOption = packet.ReadBool();

            // TODO item option check

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.UserEmotion);

            response.WriteInt(user.ID);
            response.WriteInt(emotion);
            response.WriteInt(duration);
            response.WriteBool(byItemOption);

            await user.FieldSplit.Dispatch(user, response);
        }
    }
}
