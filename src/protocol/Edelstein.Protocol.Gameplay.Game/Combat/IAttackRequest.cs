﻿namespace Edelstein.Protocol.Gameplay.Game.Combat;

public interface IAttackRequest
{
    AttackType Type { get; }
    
    int DamagePerMob { get; }
    int MobCount { get; }
    
    int SkillID { get; }
    int Keydown { get; }
    
    bool IsFinalAfterSlashBlast { get; }
    bool IsSoulArrow { get; }
    bool IsShadowPartner { get; }
    bool IsSerialAttack { get; }
    bool IsSpiritJavelin { get; }
    
    bool IsLeft { get; }
    
    int Action { get; }
    
    byte AttackType { get; }
    
    int PartyCount { get; }
    int SpeedDegree { get; }
    
    int AttackTime { get; }
    
    int Phase { get; }
    
    ICollection<IAttackRequestEntry> Entries { get; }
}
