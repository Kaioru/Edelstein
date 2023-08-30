using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public class FieldMob : AbstractFieldControllable<IFieldMobMovePath, IFieldMobMoveAction>, IFieldMob, IPacketWritable
{

    public FieldMob(
        IMobTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        bool isFacingLeft = true
    ) : base(new FieldMobMoveAction(template.MoveAbility, isFacingLeft), position, foothold) =>
        Template = template;
    public override FieldObjectType Type => FieldObjectType.Mob;

    public IMobTemplate Template { get; }

    public override IPacket GetEnterFieldPacket() => GetEnterFieldPacket(FieldMobAppearType.Normal);

    public override IPacket GetLeaveFieldPacket()
    {
        using var packet = new PacketWriter(PacketSendOperations.MobLeaveField);

        packet.WriteInt(ObjectID ?? 0);
        packet.WriteInt(1); // m_tLastUpdateAmbush?
        return packet.Build();
    }

    public void WriteTo(IPacketWriter writer) => WriteTo(writer, FieldMobAppearType.Normal);

    private IPacket GetEnterFieldPacket(FieldMobAppearType appear, int? appearOption = null)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobEnterField);

        packet.WriteInt(ObjectID ?? 0);
        WriteTo(packet, appear, appearOption);
        return packet.Build();
    }

    private void WriteTo(IPacketWriter writer, FieldMobAppearType appear, int? appearOption = null)
    {
        writer.WriteByte(1); // CalcDamageStatIndex
        writer.WriteInt(Template.ID);

        writer.WriteLong(0);
        writer.WriteLong(0);

        writer.WritePoint2D(Position);
        writer.WriteByte(Action.Raw);
        writer.WriteShort((short)(Foothold?.ID ?? 0));
        writer.WriteShort(0); // Foothold again? TODO

        writer.WriteByte((byte)appear);
        if (appear is FieldMobAppearType.Revived or >= 0)
            writer.WriteInt(appearOption ?? 0);

        writer.WriteByte(0);
        writer.WriteInt(0);
        writer.WriteInt(0);
    }

    protected override IPacket GetMovePacket(IFieldMobMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobMove);

        packet.WriteInt(ObjectID!.Value);
        packet.Write(ctx);

        return packet.Build();
    }

    protected override IPacket GetControlPacket(IFieldController? controller = null)
    {
        using var packet = new PacketWriter(PacketSendOperations.MobChangeController);

        packet.WriteBool(controller != null);
        packet.WriteInt(ObjectID ?? 0);

        if (controller != null)
            WriteTo(packet, FieldMobAppearType.Regen);
        return packet.Build();
    }
}
