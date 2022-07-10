using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers
{
    public class NPCMoveHandler : AbstractControlledPacketHandler<IFieldObjNPC>
    {
        public override short Operation => (short)PacketRecvOperations.NpcMove;
        public override FieldObjType Type => FieldObjType.NPC;

        protected override async Task Handle(
            GameStageUser stageUser,
            IFieldObjUser controller,
            IFieldObjNPC controlled,
            IPacketReader packet
        )
        {
            var movement = new UnstructuredOutgoingPacket(PacketSendOperations.NpcMove);

            movement.WriteInt(controlled.ID);
            movement.WriteByte(packet.ReadByte()); // TODO: actions
            movement.WriteByte(packet.ReadByte());

            if (controlled.Info.Move)
            {
                var path = packet.Read(new MovePath());

                movement.Write(path);
                await controlled.Move(path);
            }

            await controlled.FieldSplit.Dispatch(movement);
        }
    }
}
