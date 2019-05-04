using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.Mobs
{
    public partial class FieldMob
    {
        private async Task OnMobMove(IPacket packet)
        {
            var mobCtrlSN = packet.Decode<short>();
            var v7 = packet
                .Decode<byte>(); //v85 = nDistance | 4 * (v184 | 2 * ((unsigned __int8)retaddr | 2 * v72)); [ CONFIRMED ]

            var oldSplit = (v7 & 0xF0) != 0; //this is a type of CFieldSplit
            var mobMoveStartResult = (v7 & 0xF) != 0;

            var curSplit = packet.Decode<byte>();
            var illegalVelocity = packet.Decode<int>();
            var v8 = packet.Decode<byte>();

            var cheatedRandom = (v8 & 0xF0) != 0;
            var cheatedCtrlMove = (v8 & 0xF) != 0;

            var multiTargetForBall = packet.Decode<int>();
            for (var i = 0; i < multiTargetForBall; i++) packet.Decode<long>(); // int, int

            var randTimeForAreaAttack = packet.Decode<int>();
            for (var i = 0; i < randTimeForAreaAttack; i++) packet.Decode<int>();

            packet.Decode<int>(); // HackedCode
            packet.Decode<int>(); // idk
            packet.Decode<int>(); // HackedCodeCrc
            packet.Decode<int>(); // idk

            using (var p = new Packet(SendPacketOperations.MobCtrlAck))
            {
                p.Encode<int>(ID);
                p.Encode<short>(mobCtrlSN);
                p.Encode<bool>(mobMoveStartResult);
                p.Encode<short>(0); // nMP
                p.Encode<byte>(0); // SkillCommand
                p.Encode<byte>(0); // SLV

                await Controller.SendPacket(p);
            }

            using (var p = new Packet(SendPacketOperations.MobMove))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(mobMoveStartResult);
                p.Encode<byte>(curSplit);
                p.Encode<byte>(0); // not sure
                p.Encode<byte>(0); // not sure
                p.Encode<int>(illegalVelocity);

                p.Encode<int>(0); // MultiTargetForBall
                p.Encode<int>(0); // RandTimeForAreaAttack

                Move(packet).Encode(p);

                await Field.BroadcastPacket(Controller, p);
            }
        }
    }
}