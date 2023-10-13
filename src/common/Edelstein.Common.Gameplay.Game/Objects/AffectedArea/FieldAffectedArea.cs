using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.AffectedArea;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Objects.AffectedArea;

public class FieldAffectedArea : AbstractFieldObject, IFieldAffectedArea, ITickable
{
    private readonly ICollection<IFieldObject> _affected;
    
    public FieldAffectedArea(
        int ownerID, 
        AffectedAreaType areaType, 
        int skillID, 
        int skillLevel, 
        int info, 
        int phase, 
        IRectangle2D bounds, 
        DateTime? dateStart = null, 
        DateTime? dateExpire = null
    ) : base(bounds.Center)
    {
        _affected = new HashSet<IFieldObject>();
        
        OwnerID = ownerID;
        AreaType = areaType;
        SkillID = skillID;
        SkillLevel = skillLevel;
        Info = info;
        Phase = phase;
        Bounds = bounds;
        DateStart = dateStart;
        DateExpire = dateExpire;
        Actions = new List<IFieldAffectedAreaAction>();
    }

    public override FieldObjectType Type => FieldObjectType.AffectedArea;
    
    public int OwnerID { get; }
    
    public AffectedAreaType AreaType { get; }

    public int SkillID { get; }
    public int SkillLevel { get; }
    
    public int Info { get; }
    public int Phase { get; }
    
    public IRectangle2D Bounds { get; }
    
    public DateTime? DateStart { get; }
    public DateTime? DateExpire { get; }
    
    public ICollection<IFieldAffectedAreaAction> Actions { get; }

    public Task Enter(IFieldObject obj)
    {
        foreach (var action in Actions)
            _ = action.OnEnter(obj);
        return Task.CompletedTask;
    }

    public Task Leave(IFieldObject obj)
    {
        foreach (var action in Actions)
            _ = action.OnLeave(obj);
        return Task.CompletedTask;
    }

    public override IPacket GetEnterFieldPacket()
    {
        using var packet = new PacketWriter(PacketSendOperations.AffectedAreaCreated);

        packet.WriteInt(ObjectID ?? 0);
        
        packet.WriteInt((int)AreaType);
        packet.WriteInt(OwnerID);
        
        packet.WriteInt(SkillID);
        packet.WriteByte((byte)SkillLevel);

        packet.WriteShort((short)(DateStart != null ? (DateTime.UtcNow - DateStart.Value).Seconds : 0)); // tStart

        packet.WriteInt(Bounds.Left);
        packet.WriteInt(Bounds.Top);
        packet.WriteInt(Bounds.Right);
        packet.WriteInt(Bounds.Bottom);

        packet.WriteInt(Info);
        packet.WriteInt(Phase);
        
        return packet.Build();
    }
    
    public override IPacket GetLeaveFieldPacket()
    {
        using var packet = new PacketWriter(PacketSendOperations.AffectedAreaRemoved);

        packet.WriteInt(ObjectID ?? 0);
        
        return packet.Build();
    }
    
    public async Task OnTick(DateTime now)
    {
        if (Field == null) return;
        if (now > DateExpire)
            await Field.Leave(this);

        foreach (var obj in _affected
                     .Where(o => o is not IFieldAffectedArea)
                     .Where(o => _affected.Contains(o))
                     .Where(o => !Bounds.Intersects(o.Position) || o.Field != Field)
                     .ToImmutableArray())
        {
            _affected.Remove(obj);
            _ = Leave(obj);
        }

        if (Field == null) return;

        foreach (var obj in Field
                     .GetSplits(Bounds)
                     .Where(s => s != null)
                     .SelectMany(s => s!.Objects)
                     .Where(o => o is not IFieldAffectedArea)
                     .Where(o => !_affected.Contains(o))
                     .Where(o => Bounds.Intersects(o.Position))
                     .ToImmutableArray())
        {
            _affected.Add(obj);
            _ = Enter(obj);
        }

        foreach (var action in Actions)
        foreach (var obj in _affected)
            _ = action.OnTick(obj);
    }
}
