using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers
{
    public class NPCMoveHandler : AbstractControlledPacketHandler<IFieldObjNPC>
    {
        public override short Operation => (short)PacketRecvOperations.NpcMove;

        protected override async Task Handle(
            GameStageUser stageUser,
            IFieldObjUser controller,
            IFieldObjNPC controlled,
            IPacketReader packet
        )
        {
            var response = new UnstructuredOutgoingPacket(PacketSendOperations.NpcMove);

            response.WriteInt(controlled.ID);
            response.WriteByte(packet.ReadByte()); // TODO: actions
            response.WriteByte(packet.ReadByte());

            if (controlled.Template.Move)
                await controlled.Move(packet.Read(new MovePath()));
            await controlled.FieldSplit.Dispatch(response);
        }
    }
}
