﻿using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Utilities;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
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
    
    public async Task<IUserAttackDamage[]> CalculatePDamage(ICharacter character, IFieldUserStats stats, IFieldMobStats target, IUserAttack attack)
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        var skill = attack.SkillID > 0 ? await _skills.Retrieve(attack.SkillID) : null;
        var skillLevel = skill?.Levels[attack.SkillLevel];
        var attackCount = skillLevel?.AttackCount ?? 1;
        var result = new IUserAttackDamage[attackCount];

        _rndGenForCharacter.Next(random.Array);
        
        var sqrtACC = Math.Sqrt(stats.PACC);
        var sqrtEVA = Math.Sqrt(target.EVA);
        var hitRate = sqrtACC - sqrtEVA + 100 + stats.Ar * (sqrtACC - sqrtEVA + 100) / 100;

        hitRate = Math.Min(hitRate, 100);

        if (target.Level > stats.Level)
            hitRate -= 5 * (target.Level - stats.Level);
        
        for (var i = 0; i < attackCount; i++)
        {
            random.Skip();

            if (hitRate < GetRandomInRange(random.Next(), 0, 100))
            {
                result[i] = new UserAttackDamage(0);
                continue;
            }

            var damage = GetRandomInRange(random.Next(), stats.DamageMin, stats.DamageMax);
            var critical = false;

            if (target.Level > stats.Level)
                damage *= (100d - (target.Level - stats.Level)) / 100d;

            damage *= (100d - (target.PDR * stats.IMDr / -100 + target.PDR)) / 100d;

            if (skill != null && skillLevel != null)
            {
                var skillDamage = skillLevel.Damage;

                if (skill.ID is Skill.WarriorPowerStrike or Skill.WarriorSlashBlast)
                {
                    var incSkills = new List<int>{
                        Skill.FighterImproveBasic,
                        Skill.PageImproveBasic,
                        Skill.SpearmanImproveBasic
                    };

                    foreach (var incSkill in incSkills)
                    {
                        var level = character.Skills[incSkill]?.Level ?? 0;

                        if (level <= 0) continue;
                        
                        var incSkillTemplate = await _skills.Retrieve(incSkill);
                        var incSkillLevel = incSkillTemplate?.Levels[level];

                        if (incSkillLevel == null) break;
                        
                        skillDamage += skill.ID == Skill.WarriorPowerStrike 
                            ? incSkillLevel.X 
                            : incSkillLevel.Y;
                        break;
                    }
                }
                
                damage *= skillDamage / 100d;
            }

            if (stats.Cr > 0 && GetRandomInRange(random.Next(), 0, 100) <= stats.Cr)
            {
                var cd = (int)GetRandomInRange(random.Next(), stats.CDMin, stats.CDMax) / 100d;

                damage += (int)damage * cd;
                critical = true;
            }

            damage = Math.Min(Math.Max(damage, 1), 999999);
            result[i] = new UserAttackDamage((int)damage, critical);
        }
        
        return result;
    }
    
    public async Task<IUserAttackDamage[]> CalculateMDamage(ICharacter character, IFieldUserStats stats, IFieldMobStats target, IUserAttack attack)    
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
