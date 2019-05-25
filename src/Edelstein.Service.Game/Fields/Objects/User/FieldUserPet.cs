using System.Drawing;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User
{
    public class FieldUserPet : AbstractFieldLife, IFieldOwnedObj
    {
        public override FieldObjType Type => FieldObjType.Etc;

        public IFieldUser Owner { get; }
        public ItemSlotPet Item { get; }
        public byte IDx { get; set; }
        public bool NameTag { get; }
        public bool ChatBalloon { get; }

        public FieldUserPet(IFieldUser owner, ItemSlotPet item, byte idx)
        {
            Owner = owner;
            Item = item;
            IDx = idx;

            Field = owner.Field;
            Position = owner.Position;
            Foothold = owner.Foothold;
        }

        public void EncodeData(IPacket packet)
        {
            packet.Encode<int>(Item.TemplateID);
            packet.Encode<string>(Item.PetName);
            packet.Encode<long>(Item.CashItemSN ?? 0);
            packet.Encode<Point>(Position);
            packet.Encode<byte>(MoveAction);
            packet.Encode<short>(Foothold);
            packet.Encode<bool>(NameTag);
            packet.Encode<bool>(ChatBalloon);
        }

        public override IPacket GetEnterFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.PetActivated))
            {
                p.Encode<int>(Owner.ID);
                p.Encode<byte>(IDx);

                p.Encode<bool>(true);
                p.Encode<bool>(true);

                EncodeData(p);
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
            => GetLeaveFieldPacket(0x0);

        public IPacket GetLeaveFieldPacket(byte leaveType)
        {
            using (var p = new Packet(SendPacketOperations.PetActivated))
            {
                p.Encode<int>(Owner.ID);
                p.Encode<byte>(IDx);

                p.Encode<bool>(false);
                p.Encode<byte>(leaveType);
                return p;
            }
        }
    }
}