using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.MessageBox;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.MessageBox
{
    public class FieldObjMessageBox : AbstractFieldObj, IFieldObjMessageBox, ITickerBehavior
    {
        public override FieldObjType Type => FieldObjType.MessageBox;

        public int ItemID { get; init; }
        public string Hope { get; init; }
        public string Name { get; init; }

        public DateTime? DateExpire { get; init; }

        public override IPacket GetEnterFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MessageBoxEnterField);

            packet.WriteInt(ID);

            packet.WriteInt(ItemID);
            packet.WriteString(Hope);
            packet.WriteString(Name);

            packet.WritePoint2D(Position);
            return packet;
        }

        public override IPacket GetLeaveFieldPacket() => GetLeaveFieldPacket(true);

        public IPacket GetLeaveFieldPacket(bool isSplitMigrate)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MessageBoxLeaveField);

            packet.WriteBool(isSplitMigrate);
            packet.WriteInt(ID);
            return packet;
        }

        public async Task OnTick(DateTime now)
        {
            if (DateExpire == null) return;
            if (Field == null) return;
            if (now > DateExpire) await Field.Leave(this, () => GetLeaveFieldPacket(false));
        }
    }
}
