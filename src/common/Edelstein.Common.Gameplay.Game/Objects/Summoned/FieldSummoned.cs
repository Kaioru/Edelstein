using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Summoned;

public class FieldSummoned : AbstractFieldLife<IFieldSummonedMovePath, IFieldSummonedMoveAction>, IFieldSummoned
{
    public FieldSummoned(
        IFieldUser owner, 
        int skillID, 
        byte skillLevel, 
        MoveAbilityType moveAbility, 
        SummonedAssistType assistType, 
        IPoint2D position,
        IFieldFoothold? foothold = null
    ) : base(new FieldSummonedMoveAction(Convert.ToByte(owner.Action.Direction == MoveActionDirection.Left)), position, foothold)
    {
        Owner = owner;
        SkillID = skillID;
        SkillLevel = skillLevel;
        MoveAbility = moveAbility;
        AssistType = assistType;
    }
    
    public IFieldUser Owner { get; }

    public int SkillID { get; }
    public byte SkillLevel { get; }
    
    public MoveAbilityType MoveAbility { get; }
    public SummonedAssistType AssistType { get; }
    
    public override FieldObjectType Type => FieldObjectType.Summoned;
    
    public Task Move(IPoint2D position)
        => Move(position, true);
    
    public override IPacket GetEnterFieldPacket()
        => GetEnterFieldPacket(0);
    
    public override IPacket GetLeaveFieldPacket()
        => GetEnterFieldPacket(3);

    public IPacket GetEnterFieldPacket(byte enterType)
    {
        using var packet = new PacketWriter(PacketSendOperations.SummonedEnterField);

        packet.WriteInt(Owner.Character.ID);
        packet.WriteInt(ObjectID!.Value);

        packet.WriteInt(SkillID);
        packet.WriteByte(Owner.Character.Level);
        packet.WriteByte(SkillLevel);

        packet.WritePoint2D(Position);
        packet.WriteByte(Action.Raw);
        packet.WriteShort((short)(Foothold?.ID ?? 0));

        packet.WriteByte((byte)MoveAbility);
        packet.WriteByte((byte)AssistType);
        
        packet.WriteByte(enterType);

        packet.WriteBool(false); // AvatarLook

        return packet.Build();
    }
    
    public IPacket GetLeaveFieldPacket(byte leaveType)
    {
        using var packet = new PacketWriter(PacketSendOperations.SummonedLeaveField);

        packet.WriteInt(Owner.Character.ID);
        packet.WriteInt(ObjectID!.Value);
        packet.WriteByte(leaveType);

        return packet.Build();
    }

    protected override IPacket GetMovePacket(IFieldSummonedMovePath ctx)
    {
        using var packet = new PacketWriter(PacketSendOperations.SummonedMove);

        packet.WriteInt(Owner.Character.ID);
        packet.WriteInt(ObjectID!.Value);
        packet.Write(ctx);

        return packet.Build();
    }
}
