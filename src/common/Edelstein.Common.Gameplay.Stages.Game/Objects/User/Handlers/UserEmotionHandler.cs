using System;
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
            var byItem = packet.ReadBool();

            var emotionPacket = new UnstructuredOutgoingPacket(PacketSendOperations.UserEmotion);

            emotionPacket.WriteInt(user.ID);
            emotionPacket.WriteInt(emotion);
            emotionPacket.WriteInt(duration);
            emotionPacket.WriteBool(byItem);

            await user.FieldSplit.Dispatch(emotionPacket);
        }
    }
}
