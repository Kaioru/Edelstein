using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Movements;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob;

public class FieldMob : AbstractFieldControllable<IMobMovePath, IMobMoveAction>, IFieldMob, IPacketWritable
{
    public FieldMob(
        IMobTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        bool isFacingLeft = true
    ) : base(new MobMoveAction(template.MoveAbility, isFacingLeft), position, foothold) =>
        Template = template;

    public override FieldObjectType Type => FieldObjectType.Mob;

    public IMobTemplate Template { get; }

    public override IPacket GetEnterFieldPacket() => GetEnterFieldPacket(FieldMobAppearType.Normal);

    public override IPacket GetLeaveFieldPacket()
    {
        var packet = new PacketWriter();

        packet.WriteInt(ObjectID ?? 0);
        packet.WriteInt(1); // m_tLastUpdateAmbush?
        return packet;
    }

    public void WriteTo(IPacketWriter writer) => WriteTo(writer, FieldMobAppearType.Normal);

    private IPacket GetEnterFieldPacket(FieldMobAppearType appear, int? appearOption = null)
    {
        var packet = new PacketWriter();

        packet.WriteInt(ObjectID ?? 0);
        WriteTo(packet, appear, appearOption);
        return packet;
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

    protected override IPacket GetMovePacket(IMobMovePath ctx) => throw new NotImplementedException();

    protected override IPacket GetControlPacket(IFieldController? controller = null)
    {
        var packet = new PacketWriter();

        packet.WriteBool(controller != null);
        packet.WriteInt(ObjectID ?? 0);

        if (controller != null)
            WriteTo(packet, FieldMobAppearType.Regen);
        return packet;
    }
}
