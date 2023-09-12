using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public class SummonedAttack : ISummonedAttack, IPacketReadable
{
    public byte MobCount { get; private set; }
    
    public byte AttackActionAndDir { get; private set; }
    
    public IPoint2D PositionOwner { get; private set; }
    public IPoint2D PositionSummoned { get; private set; }
    
    public int RepeatSkillPoint { get; private set; }
    
    public IAttackMobEntry[] MobEntries { get; private set; }
    
    public void ReadFrom(IPacketReader reader)
    {
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        
        _ = reader.ReadInt();
        
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        
        AttackActionAndDir = reader.ReadByte();
        
        _ = reader.ReadInt();
        _ = reader.ReadInt();

        MobCount = reader.ReadByte();

        PositionOwner = reader.ReadPoint2D();
        PositionSummoned = reader.ReadPoint2D();

        RepeatSkillPoint = reader.ReadInt();

        MobEntries = new IAttackMobEntry[MobCount];
        for (var i = 0; i < MobCount; i++)
            MobEntries[i] = reader.Read(new AttackMobEntry(1, true));
        
        _ = reader.ReadInt();
    }
}
