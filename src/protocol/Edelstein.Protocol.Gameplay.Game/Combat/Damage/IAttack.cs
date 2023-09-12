using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Combat.Damage;

public interface IAttack
{
    AttackType Type { get; }
    
    byte DamagePerMob { get; }
    byte MobCount { get; }
    
    int SkillID { get; }
    bool IsCombatOrders { get; }
    int Keydown { get; }
    
    byte Option { get; }
    bool IsFinalAfterSlashBlast => (Option & 0x1) > 0;
    bool IsSoulArrow => (Option & 0x2) > 0;
    bool IsShadowPartner => (Option & 0x8) > 0;
    bool IsSerialAttack => (Option & 0x20) > 0;
    bool IsSpiritJavelin => (Option & 0x40) > 0;
    bool IsSpark => (Option & 0x80) > 0;
    
    bool IsNextShootJablin { get; }
    
    short AttackActionAndDir { get; }
    short AttackAction => (short)(AttackActionAndDir & 0x7FFF);
    bool AttackIsLeft =>  (AttackActionAndDir >> 15 & 1) > 0;
    byte AttackActionType { get; }
    byte AttackSpeed { get; }
    int AttackTime { get; }
    
    int Phase { get; }
    
    short BulletItemPos { get; }
    short BulletItemPosCash { get; }
    byte ShootRange { get; }
    int SpiritJavelinItemID { get; }
    
    IAttackMobEntry[] MobEntries { get; }
    
    IPoint2D Position { get; }
}
