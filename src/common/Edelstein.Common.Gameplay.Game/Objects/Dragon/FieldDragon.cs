using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Dragon;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Dragon;

public class FieldDragon : 
    AbstractFieldLife<IFieldDragonMovePath, IFieldDragonMoveAction>, 
    IFieldDragon
{
    public FieldDragon(
        IFieldUser owner,
        short jobCode,
        IFieldDragonMoveAction action, 
        IPoint2D position, 
        IFieldFoothold? foothold = null
    ) : base(action, position, foothold)
    {
        Owner = owner;
        JobCode = jobCode;
    }

    public override FieldObjectType Type => FieldObjectType.Etc;
    
    public IFieldUser Owner { get; }
    public short JobCode { get; }
    
    public Task Move(IPoint2D position)
        => Move(position, true);
    
    public override IPacket GetEnterFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.DragonEnterField);

        packet.WriteInt(Owner.Character.ID);
        packet.WriteInt(Position.X);
        packet.WriteInt(Position.Y);
        packet.WriteByte(Action.Raw);
        packet.WriteShort((short)(Foothold?.ID ?? 0));
        packet.WriteShort(JobCode);
        return packet.Build();
    }

    public override IPacket GetLeaveFieldPacket()
    {
        var packet = new PacketWriter(PacketSendOperations.DragonLeaveField);
        
        packet.WriteInt(Owner.Character.ID);
        return packet.Build();
    }

    protected override IPacket GetMovePacket(IFieldDragonMovePath ctx)
    {
        var packet = new PacketWriter(PacketSendOperations.DragonMove);
        
        packet.WriteInt(Owner.Character.ID);
        packet.Write(ctx);
        return packet.Build();
    }
}
