using Edelstein.Common.Constants;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public class Attack : IAttack, IPacketReadable
{
    public Attack(AttackType type) 
        => Type = type;

    public AttackType Type { get; }
    
    public byte DamagePerMob { get; private set; }
    public byte MobCount { get; private set; }
    
    public int SkillID { get; private set; }
    public bool IsCombatOrders { get; private set; }
    public int Keydown { get; private set; }
    
    public byte Option { get; private set; }
    
    public bool IsNextShootJablin { get; private set; }
    
    public short AttackActionAndDir { get; private set; }
    public AttackActionType AttackActionType { get; private set; }
    public byte AttackSpeed { get; private set; }
    public int AttackTime { get; private set; }
    
    public int Phase { get; private set; }
    
    public short BulletItemPos { get; private set; }
    public short BulletItemPosCash { get; private set; }
    public byte ShootRange { get; private set; }
    public int SpiritJavelinItemID { get; private set; }
    
    public IAttackMobEntry[] MobEntries { get; private set; }

    public IPoint2D Position { get; private set; }
    
    public void ReadFrom(IPacketReader reader)
    {
        _ = reader.ReadByte(); // bCurFieldKey
        _ = reader.ReadInt(); // dr0
        _ = reader.ReadInt(); // dr1

        var v6 = reader.ReadByte();
        DamagePerMob = (byte)(v6 >> 0 & 0xF);
        MobCount = (byte)(v6 >> 4 & 0xF);
        
        _ = reader.ReadInt(); // dr2
        _ = reader.ReadInt(); // dr3

        SkillID = reader.ReadInt();
        IsCombatOrders = reader.ReadBool();
        
        _ = reader.ReadInt(); // dr rand
        _ = reader.ReadInt(); // crc

        switch (Type)
        {
            case AttackType.Magic:
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                break;
            case AttackType.Melee:
            case AttackType.Shoot:
            case AttackType.Body:
            default:
                _ = reader.ReadInt();
                _ = reader.ReadInt();
                break;
        }
        
        Keydown = SkillConstants.IsKeydownSkill(SkillID)
            ? reader.ReadInt()
            : 0;
        
        Option = reader.ReadByte();
        
        if (Type == AttackType.Shoot)
            IsNextShootJablin = reader.ReadBool();
        
        AttackActionAndDir = reader.ReadShort();
        _ = reader.ReadInt(); 
        AttackActionType = (AttackActionType)reader.ReadByte();
        AttackSpeed = reader.ReadByte();
        AttackTime = reader.ReadInt();
        
        Phase = reader.ReadInt();
        
        if (Type == AttackType.Shoot)
        {
            BulletItemPos = reader.ReadShort();
            BulletItemPosCash = reader.ReadShort();
            ShootRange = reader.ReadByte();
            // _ = reader.ReadInt(); // SpiritJavelinItemID
        }

        MobEntries = new IAttackMobEntry[MobCount];
        for (var i = 0; i < MobCount; i++)
            MobEntries[i] = reader.Read(new AttackMobEntry(DamagePerMob));

        Position = reader.ReadPoint2D();
    }
}
