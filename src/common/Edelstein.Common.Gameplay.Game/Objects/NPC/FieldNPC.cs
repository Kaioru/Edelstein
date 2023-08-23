using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC;

public class FieldNPC : AbstractFieldControllable<IFieldNPCMovePath, IFieldNPCMoveAction>, IFieldNPC, IPacketWritable
{

    public FieldNPC(
        INPCTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        IRectangle2D? bounds = null,
        bool isFacingLeft = true,
        bool isEnabled = true
    ) : base(new FieldNPCMoveAction(Convert.ToByte(isFacingLeft)), position, foothold)
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
        using var packet = new PacketWriter(PacketSendOperations.NpcEnterField);

        packet.WriteInt(ObjectID!.Value);
        packet.Write(this);

        return packet.Build();
    }

    public override IPacket GetLeaveFieldPacket()
    {
        using var packet = new PacketWriter(PacketSendOperations.NpcLeaveField);

        packet.WriteInt(ObjectID!.Value);

        return packet.Build();
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

    protected override IPacket GetMovePacket(IFieldNPCMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.NpcMove);

        packet.WriteInt(ObjectID!.Value);
        packet.Write(ctx);

        return packet.Build();
    }

    protected override IPacket GetControlPacket(IFieldController? controller = null)
    {
        using var packet = new PacketWriter(PacketSendOperations.NpcChangeController);

        packet.WriteBool(controller != null);
        packet.WriteInt(ObjectID!.Value);

        if (controller != null)
            packet.Write(this);

        return packet.Build();
    }
}
