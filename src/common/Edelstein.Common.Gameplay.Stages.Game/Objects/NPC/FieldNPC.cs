using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;

public class FieldNPC : AbstractFieldControllable<INPCMovePath, INPCMoveAction>, IFieldNPC, IPacketWritable
{
    public FieldNPC(
        INPCTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        IRectangle2D? bounds = null,
        bool isFacingLeft = true,
        bool isEnabled = true
    ) : base(new NPCMoveAction(Convert.ToByte(isFacingLeft)), position, foothold)
    {
        Template = template;
        Bounds = bounds ?? new Rectangle2D(Position, Position);
        IsEnabled = isEnabled;
    }

    public override FieldObjectType Type => FieldObjectType.NPC;

    public INPCTemplate Template { get; }
    public IRectangle2D Bounds { get; }

    public bool IsEnabled { get; }

    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter();

        packet.WriteInt(ObjectID!.Value);
        packet.Write(this);

        return packet;
    }

    public override IPacket GetLeaveFieldPacket()
    {
        var packet = new PacketWriter();

        packet.WriteInt(ObjectID!.Value);

        return packet;
    }

    public void WriteTo(IPacketWriter writer)
    {
        writer.WriteInt(Template.ID);

        writer.WritePoint2D(Position);
        writer.WriteByte(Action.Raw);
        writer.WriteShort((short)(Foothold?.ID ?? 0));

        writer.WriteShort((short)Bounds.Left);
        writer.WriteShort((short)Bounds.Right);

        writer.WriteBool(IsEnabled);
    }

    protected override IPacket GetMovePacket(INPCMovePath ctx)
    {
        var packet = new PacketWriter();

        packet.WriteInt(ObjectID!.Value);
        packet.Write(ctx);

        return packet;
    }

    protected override IPacket GetControlPacket(IFieldController? controller = null)
    {
        var packet = new PacketWriter();

        packet.WriteBool(controller != null);
        packet.WriteInt(ObjectID!.Value);

        if (controller != null)
            packet.Write(this);

        return packet;
    }
}
