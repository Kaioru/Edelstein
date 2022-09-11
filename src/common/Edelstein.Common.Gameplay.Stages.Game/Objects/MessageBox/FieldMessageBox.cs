using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.MessageBox;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.MessageBox;

public class FieldMessageBox : AbstractFieldObject, IFieldMessageBox
{
    public FieldMessageBox(IPoint2D position, int itemID, string hope, string name) : base(position)
    {
        ItemID = itemID;
        Hope = hope;
        Name = name;
    }

    public int ItemID { get; }
    public string Hope { get; }
    public string Name { get; }

    public override FieldObjectType Type => FieldObjectType.MessageBox;

    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter();

        packet.WriteInt(ObjectID ?? 0);

        packet.WriteInt(ItemID);
        packet.WriteString(Hope);
        packet.WriteString(Name);

        packet.WritePoint2D(Position);

        return packet;
    }

    public override IPacket GetLeaveFieldPacket() => GetLeaveFieldPacket(true);

    private IPacket GetLeaveFieldPacket(bool isSplitMigrate)
    {
        var packet = new PacketWriter();

        packet.WriteBool(isSplitMigrate);
        packet.WriteInt(ObjectID ?? 0);
        return packet;
    }
}
