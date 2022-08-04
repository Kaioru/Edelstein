using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;

public class FieldNPC : AbstractFieldLife, IFieldNPC, IPacketWritable
{
    public FieldNPC(
        INPCTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        int rx0 = 0,
        int rx1 = 0,
        bool isFacingLeft = true,
        bool isEnabled = true
    ) : base(position, foothold)
    {
        Template = template;
        RX0 = rx0;
        RX1 = rx1;
        Action = Convert.ToByte(isFacingLeft);
        IsEnabled = isEnabled;
    }

    public override FieldObjectType Type => FieldObjectType.NPC;

    public INPCTemplate Template { get; }

    public bool IsEnabled { get; }

    public int RX0 { get; }
    public int RX1 { get; }

    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.NpcEnterField);

        packet.WriteInt(ObjectID!.Value);
        packet.Write(this);

        return packet;
    }

    public override IPacket GetLeaveFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.NpcLeaveField);

        packet.WriteInt(ObjectID!.Value);

        return packet;
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt(Template.ID);

        writer.WritePoint2D(Position);
        writer.WriteByte(Action);
        writer.WriteShort((short)(Foothold?.ID ?? 0));

        writer.WriteShort((short)RX0);
        writer.WriteShort((short)RX1);

        writer.WriteBool(IsEnabled);
    }

    protected override IPacket GetMovePacket(IMovePath ctx) => throw new NotImplementedException();
}
