using System;
using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Fields.Objects.Drop
{
    public abstract class AbstractFieldDrop : AbstractFieldObj, ITickable
    {
        public override FieldObjType Type => FieldObjType.Drop;
        public abstract bool IsMoney { get; }
        public abstract int Info { get; }

        public DateTime? DateExpire { get; set; }

        public abstract Task PickUp(FieldUser user);

        public IPacket GetEnterFieldPacket(
            byte enterType,
            IFieldObj source = null,
            short delay = 0
        )
        {
            using (var p = new Packet(SendPacketOperations.DropEnterField))
            {
                p.Encode<byte>(enterType); // nEnterType
                p.Encode<int>(ID);

                p.Encode<bool>(IsMoney);
                p.Encode<int>(Info);
                p.Encode<int>(0); // dwOwnerID
                p.Encode<byte>(0x2); // nOwnType
                p.Encode<Point>(Position);
                p.Encode<int>(source is FieldUser ? 0 : source?.ID ?? 0); // dwSourceID

                if (enterType == 0x0 ||
                    enterType == 0x1 ||
                    enterType == 0x3 ||
                    enterType == 0x4)
                {
                    p.Encode<Point>(source?.Position ?? new Point(0, 0));
                    p.Encode<short>(delay);
                }

                if (!IsMoney)
                    p.Encode<long>(0);

                p.Encode<bool>(false);
                p.Encode<bool>(false);
                return p;
            }
        }

        public IPacket GetLeaveFieldPacket(
            byte leaveType,
            IFieldObj source = null
        )
        {
            using (var p = new Packet(SendPacketOperations.DropLeaveField))
            {
                p.Encode<byte>(leaveType); // nLeaveType
                p.Encode<int>(ID);

                if (leaveType == 0x2 ||
                    leaveType == 0x3 ||
                    leaveType == 0x5) p.Encode<int>(source?.ID ?? 0);
                else if (leaveType == 0x4) p.Encode<short>(0);

                return p;
            }
        }

        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(0x2);

        public override IPacket GetLeaveFieldPacket()
            => GetLeaveFieldPacket(0x1);

        public async Task OnTick(DateTime now)
        {
            if (DateExpire.HasValue && now > DateExpire.Value)
                await Field.Leave(this);
        }
    }
}