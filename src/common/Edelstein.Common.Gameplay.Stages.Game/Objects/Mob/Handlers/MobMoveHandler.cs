using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Handlers
{
    public class MobMoveHandler : AbstractControlledPacketHandler<IFieldObjMob>
    {
        public override short Operation => (short)PacketRecvOperations.MobMove;
        public override FieldObjType Type => FieldObjType.Mob;

        protected override async Task Handle(
            GameStageUser stageUser,
            IFieldObjUser controller,
            IFieldObjMob controlled,
            IPacketReader packet
        )
        {
            if (controlled.HP <= 0) return;

            var mobCtrlSN = packet.ReadShort();
            var v7 = packet.ReadByte(); //v85 = nDistance | 4 * (v184 | 2 * ((unsigned __int8)retaddr | 2 * v72)); [ CONFIRMED ]

            var oldSplit = (v7 & 0xF0) != 0; //this is a type of CFieldSplit
            var mobMoveStartResult = (v7 & 0xF) != 0;

            var curSplit = packet.ReadByte();
            var illegalVelocity = packet.ReadInt();
            var v8 = packet.ReadByte();

            var cheatedRandom = (v8 & 0xF0) != 0;
            var cheatedCtrlMove = (v8 & 0xF) != 0;

            var multiTargetForBall = packet.ReadInt();
            for (var i = 0; i < multiTargetForBall; i++) packet.ReadLong(); // int, int

            var randTimeForAreaAttack = packet.ReadInt();
            for (var i = 0; i < randTimeForAreaAttack; i++) packet.ReadInt();

            packet.ReadInt(); // HackedCode
            packet.ReadInt(); // idk
            packet.ReadInt(); // HackedCodeCrc
            packet.ReadInt(); // idk

            var path = packet.Read(new MovePath());

            await controlled.Move(path);

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.MobCtrlAck);

            response.WriteInt(controlled.ID);
            response.WriteShort(mobCtrlSN);
            response.WriteBool(mobMoveStartResult);
            response.WriteShort((short)controlled.MP); // nMP
            response.WriteByte(0); // SkillCommand
            response.WriteByte(0); // SLV

            await controller.Dispatch(response);

            var movement = new UnstructuredOutgoingPacket(PacketSendOperations.MobMove);

            movement.WriteInt(controlled.ID);
            movement.WriteBool(false); // NotForceLandingWhenDiscard
            movement.WriteBool(false); // NotChangeAction
            movement.WriteBool(false); // NextAttackPossible
            movement.WriteBool(false); // Left
            movement.WriteInt(illegalVelocity);

            movement.WriteInt(0); // MultiTargetForBall
            movement.WriteInt(0); // RandTimeForAreaAttack

            movement.Write(path);

            await controlled.Field.Dispatch(controller, movement);
        }
    }
}
