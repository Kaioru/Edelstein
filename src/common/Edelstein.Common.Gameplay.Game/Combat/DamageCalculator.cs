﻿using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Utilities;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class DamageCalculator : IDamageCalculator
{
    private const int RndSize = 7;
    
    private readonly DamageRand _rndGenForCharacter;
    private readonly DamageRand _rndForCheckDamageMiss;
    private readonly DamageRand _rndGenForMob;

    private readonly ITemplateManager<ISkillTemplate> _skills;

    public DamageCalculator(ITemplateManager<ISkillTemplate> skills)
    {
        var random = new Random();
        var s1 = (uint)random.Next();
        var s2 = (uint)random.Next();
        var s3 = (uint)random.Next();
        
        InitSeed1 = s1;
        InitSeed2 = s2;
        InitSeed3 = s3;

        s1 |= 0x100000;
        s2 |= 0x1000;
        s3 |= 0x10;

        _rndGenForCharacter = new DamageRand(s1, s2, s3);
        _rndForCheckDamageMiss = new DamageRand(s1, s2, s3);
        _rndGenForMob = new DamageRand(s1, s2, s3);
        
        _skills = skills;
    }
    
    public uint InitSeed1 { get; }
    public uint InitSeed2 { get; }
    public uint InitSeed3 { get; }
    
    public void Skip()
        => _rndGenForCharacter.Next(new uint[RndSize]);
    
    public async Task<IUserAttackDamage[]> CalculatePDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob,
        IFieldMobStats mobStats,
        IUserAttack attack
    )
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        var skill = attack.SkillID > 0 ? await _skills.Retrieve(attack.SkillID) : null;
        var skillLevel = skill?.Levels[attack.SkillLevel];
        var attackCount = skillLevel?.AttackCount ?? 1;
        var result = new IUserAttackDamage[attackCount];

        _rndGenForCharacter.Next(random.Array);
        
        var sqrtACC = Math.Sqrt(stats.PACC);
        var sqrtEVA = Math.Sqrt(mobStats.EVA);
        var hitRate = sqrtACC - sqrtEVA + 100 + stats.Ar * (sqrtACC - sqrtEVA + 100) / 100;

        hitRate = Math.Min(hitRate, 100);

        if (mobStats.Level > stats.Level)
            hitRate -= 5 * (mobStats.Level - stats.Level);
        
        for (var i = 0; i < attackCount; i++)
        {
            random.Skip();

            if (attack.SkillID is 
                    Skill.HeroRush or 
                    Skill.PaladinRush or 
                    Skill.DarkknightRush or 
                    Skill.Dual3HustleRush or 
                    Skill.PaladinBlast or 
                    Skill.Dual4FlyingAssaulter or
                    Skill.Dual2SlashStorm or
                    Skill.Dual4BloodyStorm
            ) random.Skip();

            if (hitRate < GetRandomInRange(random.Next(), 0, 100))
            {
                result[i] = new UserAttackDamage(0);
                continue;
            }

            var damage = GetRandomInRange(random.Next(), stats.DamageMin, stats.DamageMax);
            var critical = false;

            if (mobStats.Level > stats.Level)
                damage *= (100d - (mobStats.Level - stats.Level)) / 100d;

            damage *= (100d - (mobStats.PDR * stats.IMDr / -100 + mobStats.PDR)) / 100d;
            damage += damage * stats.PDamR / 100d;

            if (mob.TemporaryStats[MobTemporaryStatType.Stun] != null ||
                mob.TemporaryStats[MobTemporaryStatType.Blind] != null ||
                mob.TemporaryStats[MobTemporaryStatType.Freeze] != null)
            {
                var chanceAttackSkill = await _skills.Retrieve(Skill.CrusaderChanceAttack);
                var chanceAttackLevel = chanceAttackSkill?.Levels[character.Skills[Skill.CrusaderChanceAttack]?.Level ?? 0];

                if (chanceAttackLevel != null) 
                    damage *= chanceAttackLevel.Damage / 100d;
            }

            if (skill != null && skillLevel != null)
            {
                var skillDamage = (int)skillLevel.Damage;

                if (skill.ID is Skill.WarriorPowerStrike or Skill.WarriorSlashBlast)
                {
                    foreach (var skillID in new List<int>{
                                 Skill.FighterImproveBasic,
                                 Skill.PageImproveBasic,
                                 Skill.SpearmanImproveBasic
                    })
                    {
                        if (character.Skills[skillID]?.Level <= 0) continue;
                        
                        var warriorImproveBasicSkill = await _skills.Retrieve(skillID);
                        var warriorImproveBasicLevel = warriorImproveBasicSkill?.Levels[character.Skills[skillID]?.Level ?? 0];

                        if (warriorImproveBasicLevel == null) break;
                        
                        skillDamage += skill.ID == Skill.WarriorPowerStrike 
                            ? warriorImproveBasicLevel.X 
                            : warriorImproveBasicLevel.Y;
                        break;
                    }
                }
                
                damage *= skillDamage / 100d;
            }

            var finalDamageR = 0.0;
            var comboCounterStat = character.TemporaryStats[TemporaryStatType.ComboCounter];
            
            if (comboCounterStat != null)
            {
                var comboCounterSkill = await _skills.Retrieve(comboCounterStat.Reason);
                var comboCounterLevel = comboCounterSkill?.Levels[character.Skills[comboCounterStat.Reason]?.Level ?? 0];
                var comboCounter = comboCounterStat.Value - 1;
                
                var advComboCounterSkillID = JobConstants.GetJobRace(character.Job) == 0
                    ? Skill.HeroAdvancedCombo
                    : Skill.SoulmasterAdvancedCombo;
                var advComboCounterSkill = await _skills.Retrieve(advComboCounterSkillID);
                var advComboCounterLevel = advComboCounterSkill?.Levels[character.Skills[advComboCounterSkillID]?.Level ?? 0];
                
                var damagePerCombo = advComboCounterLevel?.X ?? comboCounterLevel?.X ?? 0;
                    
                if (attack.SkillID is 
                    Skill.CrusaderPanic or 
                    Skill.CrusaderComa or
                    Skill.SoulmasterPanicSword or 
                    Skill.SoulmasterComaSword)
                {
                    var comboAttackSkill = await _skills.Retrieve(attack.SkillID);
                    var comboAttackLevel = comboAttackSkill?.Levels[character.Skills[attack.SkillID]?.Level ?? 0];

                    damagePerCombo += comboAttackLevel?.Y ?? 0;
                }
                
                finalDamageR += (short)(comboCounter * damagePerCombo);
            }
            
            if (stats.Cr > 0 && GetRandomInRange(random.Next(), 0, 100) <= stats.Cr)
            {
                finalDamageR += (int)GetRandomInRange(random.Next(), stats.CDMin, stats.CDMax);
                critical = true;
            }
            
            damage += damage * finalDamageR / 100d;
            damage = Math.Min(Math.Max(damage, 1), 999999);
            result[i] = new UserAttackDamage((int)damage, critical);
        }
        
        return result;
    }
    
    public async Task<IUserAttackDamage[]> CalculateMDamage(
        ICharacter character, 
        IFieldUserStats stats,
        IFieldMob mob,
        IFieldMobStats mobStats, 
        IUserAttack attack
    )    
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        var skill = attack.SkillID > 0 ? await _skills.Retrieve(attack.SkillID) : null;
        var skillLevel = skill?.Levels[attack.SkillLevel];
        var attackCount = skillLevel?.AttackCount ?? 1;
        var result = new IUserAttackDamage[attackCount];

        _rndGenForCharacter.Next(random.Array);
        
        for (var i = 0; i < attackCount; i++)
            result[i] = new UserAttackDamage(0);

        return result;
    }
    
    private double GetRandomInRange(uint rand, double f0, double f1)
    {
        if (f1 != f0)
        {
            if (f0 > f1)
            {
                var tmp = f1;
                f0 = f1;
                f1 = tmp;
            }

            return f0 + rand % 10000000 * (f1 - f0) / 9999999.0;
        }
        else return f0;
    }
}
