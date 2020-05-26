using System.Threading.Tasks;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Movements;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Handlers.Mob
{
    public class MobMoveHandler : AbstractFieldMobHandler
    {
        protected override async Task Handle(
            FieldUser user,
            FieldMob mob,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var mobCtrlSN = packet.DecodeShort();
            var v7 = packet
                .DecodeByte(); //v85 = nDistance | 4 * (v184 | 2 * ((unsigned __int8)retaddr | 2 * v72)); [ CONFIRMED ]

            var oldSplit = (v7 & 0xF0) != 0; //this is a type of CFieldSplit
            var mobMoveStartResult = (v7 & 0xF) != 0;

            var curSplit = packet.DecodeByte();
            var illegalVelocity = packet.DecodeInt();
            var v8 = packet.DecodeByte();

            var cheatedRandom = (v8 & 0xF0) != 0;
            var cheatedCtrlMove = (v8 & 0xF) != 0;

            var multiTargetForBall = packet.DecodeInt();
            for (var i = 0; i < multiTargetForBall; i++) packet.DecodeLong(); // int, int

            var randTimeForAreaAttack = packet.DecodeInt();
            for (var i = 0; i < randTimeForAreaAttack; i++) packet.DecodeInt();

            packet.DecodeInt(); // HackedCode
            packet.DecodeInt(); // idk
            packet.DecodeInt(); // HackedCodeCrc
            packet.DecodeInt(); // idk

            var path = new MovePath(packet);

            await mob.Move(path);

            using (var p = new OutPacket(SendPacketOperations.MobCtrlAck))
            {
                p.EncodeInt(mob.ID);
                p.EncodeShort(mobCtrlSN);
                p.EncodeBool(mobMoveStartResult);
                p.EncodeShort(0); // nMP
                p.EncodeByte(0); // SkillCommand
                p.EncodeByte(0); // SLV

                await user.SendPacket(p);
            }

            using (var p = new OutPacket(SendPacketOperations.MobMove))
            {
                p.EncodeInt(mob.ID);
                p.EncodeBool(mobMoveStartResult);
                p.EncodeByte(curSplit);
                p.EncodeByte(0); // not sure
                p.EncodeByte(0); // not sure
                p.EncodeInt(illegalVelocity);

                p.EncodeInt(0); // MultiTargetForBall
                p.EncodeInt(0); // RandTimeForAreaAttack

                path.Encode(p);

                await mob.Field.BroadcastPacket(user, p);
            }
        }
    }
}