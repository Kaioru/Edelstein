using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class SummonedAttackRequest : ISummonedAttackRequest, IPacketReadable
{
    public int MobCount { get; private set; }
    
    public bool IsLeft { get; private set; }
    
    public int AttackAction { get; private set; }
    
    public IPoint2D OwnerPosition { get; private set; }
    public IPoint2D SummonedPosition { get; private set; }
    
    public int RepeatSkillPoint { get; private set; }
    
    public ICollection<IAttackRequestEntry> Entries { get; } = new List<IAttackRequestEntry>();

    public void ReadFrom(IPacketReader reader)
    {
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        
        _ = reader.ReadInt();
        
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        
        var v15 = reader.ReadByte();
        IsLeft = (v15 >> 7 & 1) > 0;
        AttackAction = v15 & 0x7F;
        
        _ = reader.ReadInt();
        _ = reader.ReadInt();

        MobCount = reader.ReadByte();

        OwnerPosition = reader.ReadPoint2D();
        SummonedPosition = reader.ReadPoint2D();

        RepeatSkillPoint = reader.ReadInt();
        
        for (var i = 0; i < MobCount; i++)
            Entries.Add(reader.Read(new AttackRequestEntry(1, true)));
        
        _ = reader.ReadInt();
    }
}
