using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.AffectedArea;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.AffectedArea
{
    public class FieldObjAffectedArea : AbstractFieldObj, IFieldObjAffectedArea, ITickerBehavior
    {
        public override FieldObjType Type => FieldObjType.AffectedArea;
        public AffectedAreaType AffectedAreaType { get; init; }

        public int OwnerID { get; init; }

        public int SkillID { get; init; }
        public byte SkillLevel { get; init; }
        public short SkillDelay { get; init; }

        public Rect2D Area { get; init; }

        public int Info { get; init; }
        public int Phase { get; init; }

        public DateTime? DateExpire { get; init; }

        public override IPacket GetEnterFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.AffectedAreaCreated);

            packet.WriteInt(ID);
            packet.WriteInt((int)AffectedAreaType);

            packet.WriteInt(OwnerID);

            packet.WriteInt(SkillID);
            packet.WriteByte(SkillLevel);
            packet.WriteShort(SkillDelay);

            packet.WriteInt(Area.Left);
            packet.WriteInt(Area.Top);
            packet.WriteInt(Area.Right);
            packet.WriteInt(Area.Bottom);

            packet.WriteInt(Info);
            packet.WriteInt(Phase);
            return packet;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.AffectedAreaRemoved);

            packet.WriteInt(ID);
            return packet;
        }

        public async Task OnTick(DateTime now)
        {
            if (DateExpire == null) return;
            if (Field == null) return;
            if (now > DateExpire) await Field.Leave(this);
        }
    }
}
