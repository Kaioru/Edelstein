using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class ContiStateHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.CONTISTATE;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            var contimove = await stageUser.Stage.ContiMoveRepository.RetrieveByField(user.Field);

            if (contimove == null) return;

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CONTISTATE);

            response.WriteByte((byte)contimove.State);
            response.WriteBool(contimove.State == ContiMoveState.Event);

            await user.Dispatch(response);
        }
    }
}
