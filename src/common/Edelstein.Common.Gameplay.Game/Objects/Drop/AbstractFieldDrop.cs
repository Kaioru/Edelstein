using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Drop;

public abstract class AbstractFieldDrop : AbstractFieldObject, IFieldDrop
{
    private readonly SemaphoreSlim _lock;
    
    protected AbstractFieldDrop(
        IPoint2D position, 
        DropOwnType ownType, 
        int ownerID, 
        int sourceID
    ) : base(position)
    {
        _lock = new SemaphoreSlim(1, 1);
        OwnType = ownType;
        OwnerID = ownerID;
        SourceID = sourceID;
    }

    public override FieldObjectType Type => FieldObjectType.Drop;
    
    public abstract bool IsMoney { get; }
    public abstract int Info { get; }
    
    public DropOwnType OwnType { get; }
    public int OwnerID { get; }
    public int SourceID { get; }
    
    private bool IsPickedUp { get; set; }

    public override IPacket GetEnterFieldPacket()
        => GetEnterFieldPacket(2, Position, 0);
    public override IPacket GetLeaveFieldPacket()
        => GetLeaveFieldPacket(1, 0, 0, 0);

    private IPacket GetEnterFieldPacket(byte enterType, IPoint2D? sourcePosition = null, short z = 0)
    {
        using var packet = new PacketWriter(PacketSendOperations.DropEnterField);

        packet.WriteByte(enterType);
        packet.WriteInt(ObjectID ?? 0);

        packet.WriteBool(IsMoney);
        packet.WriteInt(Info);

        packet.WriteInt(OwnerID);
        packet.WriteByte((byte)OwnType);

        packet.WritePoint2D(Position);

        packet.WriteInt(SourceID);

        if (enterType is 0 or 1 or 3 or 4)
        {
            packet.WritePoint2D(sourcePosition ?? Position);
            packet.WriteShort(z);
        }

        return packet.Build();
    }

    private IPacket GetLeaveFieldPacket(byte leaveType, int pickupID = 0, short delay = 0, int petIndex = 0)
    {
        using var packet = new PacketWriter(PacketSendOperations.DropEnterField);

        packet.WriteByte(leaveType);
        packet.WriteInt(ObjectID ?? 0);

        if (leaveType is 2 or 3 or 5)
        {
            packet.WriteInt(pickupID);

            if (leaveType == 5)
                packet.WriteInt(petIndex);
        }

        if (leaveType == 4)
            packet.WriteShort(delay);

        return packet.Build();
    }

    public async Task Pickup(IFieldUser user)
    {
        await _lock.WaitAsync();
        
        try
        {
            if (!await Check(user))
                return;
            
            IsPickedUp = true;
            
            if (Field != null)
                await Field.Leave(this, () => GetLeaveFieldPacket(2, user.ObjectID ?? 0));
            await Update(user);
        }
        finally
        {
            _lock.Release();
        }
    }

    protected abstract Task<bool> Check(IFieldUser user);
    protected abstract Task Update(IFieldUser user);
}
