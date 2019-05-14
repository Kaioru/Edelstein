using Edelstein.Core;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.Dragons
{
    public class FieldDragon : AbstractFieldLife, IFieldOwnedObj
    {
        public override FieldObjType Type => FieldObjType.Etc;
        public IFieldUser Owner { get; }
        public short JobCode => Owner.Character.Job;

        public FieldDragon(IFieldUser owner)
        {
            Owner = owner;
        }

        public override IPacket GetEnterFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.DragonEnterField))
            {
                p.Encode<int>(Owner.ID);
                p.Encode<int>(Position.X);
                p.Encode<int>(Position.Y);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(0);
                p.Encode<short>(JobCode);
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.DragonLeaveField))
            {
                return p;
            }
        }
    }
}