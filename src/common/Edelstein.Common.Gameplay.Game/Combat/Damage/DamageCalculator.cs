using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Utilities;
using Edelstein.Protocol.Gameplay.Game.Combat.Damage;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Combat.Damage;

public class DamageCalculator : IDamageCalculator
{
    private const int RndSize = 7;
    
    private readonly DamageRand _rndGenForCharacter;
    private readonly DamageRand _rndForCheckDamageMiss;
    private readonly DamageRand _rndForMortalBlow;
    private readonly DamageRand _rndForSummoned;
    private readonly DamageRand _rndForMob;
    private readonly DamageRand _rndGenForMob;

    private readonly ITemplateManager<ISkillTemplate> _skills;

    public DamageCalculator(ITemplateManager<ISkillTemplate> skills)
    {
        var random = new Random();
        var s1 = (uint)0;
        var s2 = (uint)0;
        var s3 = (uint)0;
        
        InitSeed1 = s1;
        InitSeed2 = s2;
        InitSeed3 = s3;

        s1 |= 0x100000;
        s2 |= 0x1000;
        s3 |= 0x10;

        _rndGenForCharacter = new DamageRand(s1, s2, s3);
        _rndForCheckDamageMiss = new DamageRand(s1, s2, s3);
        _rndForMortalBlow = new DamageRand(s1, s2, s3);
        _rndForSummoned = new DamageRand(s1, s2, s3);
        _rndForMob = new DamageRand(s1, s2, s3);
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
        var skillLevel = skill?[attack.SkillLevel];
        var equipped = character.Inventories[ItemInventoryType.Equip];
        var weapon = equipped != null
            ? equipped.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) 
                ? result1.ID 
                : 0
            : 0;
        var weaponType = ItemConstants.GetWeaponType(weapon);
        var attackCount = skillLevel?.AttackCount ?? 1;

        _rndGenForCharacter.Next(random.Array);

        var darkForceSkill = await _skills.Retrieve(Skill.DarkknightDarkForce);
        var darkForceLevel = darkForceSkill?[stats.SkillLevels[Skill.DarkknightDarkForce]];
        var isDarkForce = 
            darkForceLevel != null && 
            character.Job == Job.DarkKnight &&
            character.HP >= stats.MaxHP * darkForceLevel.X / 100;

        if (isDarkForce && attack.SkillID == Skill.DragonknightDragonBurster)
            attackCount += darkForceLevel?.Y ?? 0;
        
        var result = new IUserAttackDamage[attackCount];
        
        var totalCr = stats.Cr;
        var totalCDMin = stats.CDMin;
        var totalCDMax = stats.CDMax;
        var totalIMDr = stats.IMDr;
        
        foreach (var kv in character.Skills.Records)
        {
            var psdSkillID = kv.Key;
            var skillTemplate = _skills.Retrieve(psdSkillID).Result;
            var levelTemplate = skillTemplate?[stats.SkillLevels[psdSkillID]];
            if (skillTemplate == null || levelTemplate == null) continue;
            if (!skillTemplate.IsPSD) continue;
            if (!skillTemplate.PsdSkill.Contains(attack.SkillID)) continue;
            
            totalCr += levelTemplate.Cr;
            totalCDMin += levelTemplate.CDMin;
            totalCDMax += levelTemplate.CDMax;
            totalIMDr += levelTemplate.IMDr;
        }
        
        totalCDMin = Math.Min(totalCDMin, totalCDMax);
        
        var sqrtACC = Math.Sqrt(stats.PACC);
        var sqrtEVA = Math.Sqrt(mobStats.EVA);
        var hitRate = sqrtACC - sqrtEVA + 100 + stats.Ar * (sqrtACC - sqrtEVA + 100) / 100;

        hitRate = Math.Min(hitRate, 100);

        if (mobStats.Level > stats.Level)
            hitRate -= 5 * (mobStats.Level - stats.Level);

        for (var i = 0; i < attackCount; i++)
        {
            random.Skip(); // CalcPImmune

            if (hitRate < GetRandomInRange(random.Next(), 0, 100))
            {
                result[i] = new UserAttackDamage(0);
                continue;
            }

            if (attack.SkillID is
                Skill.HeroRush or
                Skill.PaladinRush or
                Skill.DarkknightRush or
                Skill.Dual3HustleRush or
                Skill.PaladinBlast or
                Skill.Dual4FlyingAssaulter or
                Skill.Dual2SlashStorm or
                Skill.Dual4BloodyStorm)
                random.Skip();

            var damage = (double)stats.DamageMax;
            var critical = false;

            if (weaponType is WeaponType.Wand or WeaponType.Staff)
                damage *= 0.2;

            if (attack.AttackAction is 41 or 57)
                damage *= 0.1;
            
            damage = GetDamageAdjustedByRandom(damage, stats.Mastery, ItemConstants.GetMasteryConstByWeaponType(weaponType), random.Next());

            if (mobStats.Level > stats.Level)
                damage *= (100d - (mobStats.Level - stats.Level)) / 100d;

            damage += damage * stats.PDamR / 100d;

            // ElemBoost
            // Mage1 MagicComposition
            // Mage2 MagicComposition
            // Ranger FireShot
            // Sniper IceShot
            var damageAdjustedByElemAttr = skill != null
                ? GetDamageAdjustedByElemAttr(damage, mobStats.ElementAttributes[skill.Element], 1.0, 0.0)
                : damage;

            damage = damageAdjustedByElemAttr;

            var weaponChargeStat = character.TemporaryStats[TemporaryStatType.WeaponCharge];
            if (weaponChargeStat != null)
            {
                var weaponChargeSkill = await _skills.Retrieve(weaponChargeStat.Reason);
                var weaponChargeLevel = weaponChargeSkill?[stats.SkillLevels[weaponChargeStat.Reason]];

                if (weaponChargeLevel != null)
                {
                    var adjust = weaponChargeLevel.Z / 100d;
                    var amp = weaponChargeLevel.Damage / 100d;
                    var damageAdjustedByChargedElemAttr = GetDamageAdjustedByElemAttr(
                        amp * damage,
                        mobStats.ElementAttributes[SkillConstants.GetElementByChargedSkill(weaponChargeStat.Reason)],
                        adjust,
                        0.0
                    );
                    var damageAdjustedByAssistChargedElemAttr = 0;

                    damage = damageAdjustedByChargedElemAttr + damageAdjustedByAssistChargedElemAttr;
                }
            }

            if (attack.SkillID != Skill.DragonknightSacrifice &&
                attack.SkillID != Skill.ThiefmasterAssaulter &&
                attack.SkillID != Skill.ViperDemolition)
                damage *= (100d - (mobStats.PDR * totalIMDr / -100 + mobStats.PDR)) / 100d;

            var skillDamageR = !attack.IsFinalAfterSlashBlast
                ? skillLevel?.Damage ?? 100d
                : 100d;

            switch (attack.SkillID)
            {
                case Skill.WarriorPowerStrike:
                case Skill.WarriorSlashBlast:
                {
                    foreach (var skillID in new List<int>
                             {
                                 Skill.FighterImproveBasic,
                                 Skill.PageImproveBasic,
                                 Skill.SpearmanImproveBasic
                             }.Where(skillID => stats.SkillLevels[skillID] > 0))
                    {
                        var warriorImproveBasicSkill = await _skills.Retrieve(skillID);
                        var warriorImproveBasicLevel = warriorImproveBasicSkill?[stats.SkillLevels[skillID]];

                        if (warriorImproveBasicLevel == null) break;

                        skillDamageR += attack.SkillID == Skill.WarriorPowerStrike
                            ? warriorImproveBasicLevel.X
                            : warriorImproveBasicLevel.Y;
                        break;
                    }

                    break;
                }
                case Skill.KnightChargeBlow:
                {
                    var advancedChargeSkill = await _skills.Retrieve(Skill.PaladinAdvancedCharge);
                    var advancedChargeLevel = advancedChargeSkill?[stats.SkillLevels[Skill.PaladinAdvancedCharge]];

                    skillDamageR = advancedChargeLevel?.Damage ?? skillDamageR;
                    break;
                }
            }

            damage *= skillDamageR / 100d;

            var damageBefore = damage;
            var comboCounterStat = character.TemporaryStats[TemporaryStatType.ComboCounter];
            if (comboCounterStat != null)
            {
                var comboCounterSkillID = JobConstants.GetJobRace(character.Job) == 0
                    ? Skill.CrusaderComboAttack
                    : Skill.SoulmasterComboAttack;
                var comboCounterSkill = await _skills.Retrieve(comboCounterSkillID);
                var comboCounterLevel = comboCounterSkill?[stats.SkillLevels[comboCounterSkillID]];
                var comboCounter = comboCounterStat.Value - 1;
                var advComboCounterSkillID = JobConstants.GetJobRace(character.Job) == 0
                    ? Skill.HeroAdvancedCombo
                    : Skill.SoulmasterAdvancedCombo;
                var advComboCounterSkill = await _skills.Retrieve(advComboCounterSkillID);
                var advComboCounterLevel = advComboCounterSkill?[stats.SkillLevels[advComboCounterSkillID]];

                var damagePerCombo = (advComboCounterLevel?.DIPr ?? 0) + (comboCounterLevel?.DIPr ?? 0);

                if (attack.SkillID is
                    Skill.CrusaderPanic or
                    Skill.CrusaderComa or
                    Skill.SoulmasterPanicSword or
                    Skill.SoulmasterComaSword)
                {
                    var comboAttackSkill = await _skills.Retrieve(attack.SkillID);
                    var comboAttackLevel = comboAttackSkill?[stats.SkillLevels[attack.SkillID]];

                    damagePerCombo += comboAttackLevel?.Y ?? 0;
                }

                damage += damage * (comboCounter * damagePerCombo) / 100d;
            }

            var enrageStat = character.TemporaryStats[TemporaryStatType.Enrage];
            if (enrageStat?.Value / 100 > 0)
                damage *= ((enrageStat?.Value ?? 100) / 100 + 100) / 100d;

            if (stats.Cr > 0 && GetRandomInRange(random.Next(), 0, 100) <= totalCr)
            {
                var criticalDamageR = (int)GetRandomInRange(random.Next(), totalCDMin, totalCDMax);

                critical = true;
                damage += (int)damageBefore * criticalDamageR / 100d;
            }

            if (mob.TemporaryStats[MobTemporaryStatType.Stun] != null ||
                mob.TemporaryStats[MobTemporaryStatType.Blind] != null)
            {
                var chanceAttackSkill = await _skills.Retrieve(Skill.CrusaderChanceAttack);
                var chanceAttackLevel = chanceAttackSkill?[stats.SkillLevels[Skill.CrusaderChanceAttack]];

                if (chanceAttackLevel != null)
                    damage *= chanceAttackLevel.Damage / 100d;
            }

            if (mob.Template.IsBoss)
                random.Skip();
            
            if (attack.Keydown > 0)
                damage *= (90 * attack.Keydown / SkillConstants.GetMaxGaugeTime(attack.SkillID) + 10) / 100d;

            if (isDarkForce)
                damage += damage * (darkForceLevel?.Damage ?? 0) / 100d;

            if (!mob.Template.IsBoss)
            {
                if (mob.Template.MaxHP > damage && i == 0)
                    random.Skip();
            }
            
            damage = Math.Min(Math.Max(1, damage), 999999);
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
        var skillLevel = skill?[attack.SkillLevel];
        var equipped = character.Inventories[ItemInventoryType.Equip];
        var weapon = equipped != null
            ? equipped.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) 
                ? result1.ID 
                : 0
            : 0;
        var weaponType = ItemConstants.GetWeaponType(weapon);
        var attackCount = skillLevel?.AttackCount ?? 1;
        var result = new IUserAttackDamage[attackCount];

        _rndGenForCharacter.Next(random.Array);
        
        var totalCr = stats.Cr;
        var totalCDMin = stats.CDMin;
        var totalCDMax = stats.CDMax;
        var totalIMDr = stats.IMDr;
        
        foreach (var kv in character.Skills.Records)
        {
            var psdSkillID = kv.Key;
            var skillTemplate = _skills.Retrieve(psdSkillID).Result;
            var levelTemplate = skillTemplate?[stats.SkillLevels[psdSkillID]];
            if (skillTemplate == null || levelTemplate == null) continue;
            if (!skillTemplate.IsPSD) continue;
            if (!skillTemplate.PsdSkill.Contains(attack.SkillID)) continue;
            
            totalCr += levelTemplate.Cr;
            totalCDMin += levelTemplate.CDMin;
            totalCDMax += levelTemplate.CDMax;
            totalIMDr += levelTemplate.IMDr;
        }
        
        totalCDMin = Math.Min(totalCDMin, totalCDMax);
        
        var sqrtACC = Math.Sqrt(stats.MACC);
        var sqrtEVA = Math.Sqrt(mobStats.EVA);
        var hitRate = sqrtACC - sqrtEVA + 100 + stats.Ar * (sqrtACC - sqrtEVA + 100) / 100;

        hitRate = Math.Min(hitRate, 100);

        if (mobStats.Level > stats.Level)
            hitRate -= 5 * (mobStats.Level - stats.Level);

        for (var i = 0; i < attackCount; i++)
        {
            random.Skip(); // CalcMImmune

            if (hitRate < GetRandomInRange(random.Next(), 0, 100))
            {
                result[i] = new UserAttackDamage(0);
                continue;
            }
            
            var damage = GetRandomInRange(random.Next(), stats.DamageMin, stats.DamageMax);
            var critical = false;

            damage += damage * stats.MDamR / 100d;

            var elementAmpSkillID = SkillConstants.GetMagicAmplificationSkill(character.Job);
            if (elementAmpSkillID > 0)
            {
                var elementAmpSkill = await _skills.Retrieve(elementAmpSkillID);
                var elementAmpLevel = elementAmpSkill?[stats.SkillLevels[elementAmpSkillID]];

                if (elementAmpLevel != null)
                    damage *= elementAmpLevel.Y / 100d;
            }
            
            // ElemBoost
            // Mage1 MagicComposition
            // Mage2 MagicComposition
            // Ranger FireShot
            // Sniper IceShot
            var elementalResetStat = character.TemporaryStats[TemporaryStatType.ElementalReset]?.Value ?? 0;
            var damageAdjustedByElemAttr = skill != null
                ? GetDamageAdjustedByElemAttr(damage, mobStats.ElementAttributes[skill.Element], 1d - elementalResetStat / 100d, 0d)
                : damage;

            switch (attack.SkillID)
            {
                case Skill.Mage1MagicComposition:
                    damageAdjustedByElemAttr =
                        GetDamageAdjustedByElemAttr(damage * 0.5, mobStats.ElementAttributes[Element.Poison], 1.0, 0d) +
                        GetDamageAdjustedByElemAttr(damage * 0.5, mobStats.ElementAttributes[Element.Fire], 1.0, 0d);
                    break;
                case Skill.Mage2MagicComposition:
                    damageAdjustedByElemAttr =
                        GetDamageAdjustedByElemAttr(damage * 0.5, mobStats.ElementAttributes[Element.Light], 1.0, 0d) +
                        GetDamageAdjustedByElemAttr(damage * 0.5, mobStats.ElementAttributes[Element.Ice], 1.0, 0d);
                    break;
            }

            damage = damageAdjustedByElemAttr;
            damage *= (100d - (mobStats.PDR * totalIMDr / -100 + mobStats.PDR)) / 100d;
            
            var skillDamageR = skillLevel?.Damage ?? 100d;

            damage *= skillDamageR / 100d;
            
            if (stats.Cr > 0 && GetRandomInRange(random.Next(), 0, 100) <= totalCr)
            {
                var criticalDamageR = (int)GetRandomInRange(random.Next(), totalCDMin, totalCDMax);

                critical = true;
                damage += damage * criticalDamageR / 100d;
            }

            if (mob.Template.IsBoss)
                random.Skip();

            if (attack.Keydown > 0)
                damage *= (90 * attack.Keydown / SkillConstants.GetMaxGaugeTime(attack.SkillID) + 10) / 100d;

            if (!mob.Template.IsBoss)
            {
                if (mob.Template.MaxHP > damage && i == 0)
                    random.Skip();
            }
            
            damage = Math.Min(Math.Max(1, damage), 999999);
            result[i] = new UserAttackDamage((int)damage, critical);
        }

        return result;
    }

    public async Task<int> CalculatePDamage(
        ICharacter character,
        IFieldUserStats stats,
        IFieldMob mob,
        IFieldMobStats mobStats,
        IFieldSummoned summoned
    )
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        var skill = summoned.SkillID > 0 ? await _skills.Retrieve(summoned.SkillID) : null;
        var skillLevel = skill?[summoned.SkillLevel];

        _rndForSummoned.Next(random.Array);
        
        return 0;
    }

    public async Task<int> CalculateMDamage(
        ICharacter character,
        IFieldUserStats stats,
        IFieldMob mob,
        IFieldMobStats mobStats,
        IFieldSummoned summoned
    )
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        var skill = summoned.SkillID > 0 ? await _skills.Retrieve(summoned.SkillID) : null;
        var skillLevel = skill?[summoned.SkillLevel];
        var equipped = character.Inventories[ItemInventoryType.Equip];
        var weapon = equipped != null
            ? equipped.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) 
                ? result1.ID 
                : 0
            : 0;
        var weaponType = ItemConstants.GetWeaponType(weapon);

        _rndForSummoned.Next(random.Array);
        
        var sqrtACC = Math.Sqrt(stats.MACC);
        var sqrtEVA = Math.Sqrt(mobStats.EVA);
        var hitRate = sqrtACC - sqrtEVA + 100 + stats.Ar * (sqrtACC - sqrtEVA + 100) / 100;

        hitRate = Math.Min(hitRate, 100);

        if (mobStats.Level > stats.Level)
            hitRate -= 5 * (mobStats.Level - stats.Level);

        if (hitRate < GetRandomInRange(random.Array[0], 0, 100))
            return 0;
        
        var damage = stats.DamageMax * (skillLevel?.Damage ?? 100d) / 100d;
        
        var elementAmpSkillID = SkillConstants.GetMagicAmplificationSkill(character.Job);
        if (elementAmpSkillID > 0)
        {
            var elementAmpSkill = await _skills.Retrieve(elementAmpSkillID);
            var elementAmpLevel = elementAmpSkill?[stats.SkillLevels[elementAmpSkillID]];

            if (elementAmpLevel != null)
                damage *= elementAmpLevel.Y / 100d;
        }

        damage = GetDamageAdjustedByRandom(damage, stats.Mastery, ItemConstants.GetMasteryConstByWeaponType(weaponType), random.Array[2]);
        
        var elementalResetStat = character.TemporaryStats[TemporaryStatType.ElementalReset]?.Value ?? 0;
        var damageAdjustedByElemAttr = skill != null
            ? GetDamageAdjustedByElemAttr(damage, mobStats.ElementAttributes[skill.Element], 1d - elementalResetStat / 100d, 0d)
            : damage;

        damage = damageAdjustedByElemAttr;
        return (int)damage;
    }
    
    public async Task<int> CalculateBurnedDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob, 
        IFieldMobStats mobStats, 
        int skillID, 
        int skillLevel
    )
    {
        var skill = skillID > 0 ? await _skills.Retrieve(skillID) : null;
        var level = skill?[skillLevel];
        var damage = (stats.DamageMin + stats.DamageMax) / 2d;
        
        if (mobStats.Level > stats.Level)
            damage *= (100d - (mobStats.Level - stats.Level)) / 100d;
        
        var damageAdjustedByElemAttr = skill != null
            ? GetDamageAdjustedByElemAttr(damage, mobStats.ElementAttributes[skill.Element], 1.0, 0.0)
            : damage;

        damage = damageAdjustedByElemAttr;
        damage *= (100d - (mobStats.PDR * stats.IMDr / -100 + mobStats.PDR)) / 100d;
        
        var skillDamageR = level?.Dot ?? 100d;

        damage *= skillDamageR / 100d;
        
        return Math.Max(1, (int)damage);
    }

    public async Task<IUserAttackDamage[]> AdjustDamageDecRate(IFieldUserStats stats, IUserAttack attack, int count, IUserAttackDamage[] damage)
    {
        var rate = 1d;
        var result = new IUserAttackDamage[damage.Length];

        if (attack.IsFinalAfterSlashBlast)
            rate = Math.Pow(1 / 3d, count);

        if (attack.SkillID is Skill.Archmage2ChainLightning or Skill.EvanBlaze)
        {
            var damageDecSkill = await _skills.Retrieve(Skill.Archmage2ChainLightning);
            var damageDecLevel = damageDecSkill?[stats.SkillLevels[Skill.Archmage2ChainLightning]];

            rate = (100 - count * (damageDecLevel?.X ?? 0)) / 100d;
        }

        for (var i = 0; i < damage.Length; i++)
        {
            result[i] = new UserAttackDamage(
                (int)(damage[i].Damage * rate),
                damage[i].IsCritical
            );
        }
        
        return result;
    }

    private static double GetRandomInRange(uint rand, double f0, double f1)
    {
        if (Math.Abs(f0 - f1) < 0.0001) return f0;
        if (f0 > f1)
        {
            var tmp = f1;
            f0 = f1;
            f1 = tmp;
        }
        
        return f0 + rand % 10000000 * (f1 - f0) / 9999999.0;
    }

    private static double GetDamageAdjustedByRandom(double damage, int mastery, double masteryConst, uint rand)
    {
        var masteryMultiplier = mastery / 100d;

        masteryMultiplier += masteryConst;
        masteryMultiplier = Math.Min(0.95, masteryMultiplier);

        return GetRandomInRange(rand, (int)(masteryMultiplier * damage + 0.5), damage);
    }
    
    private static double GetDamageAdjustedByElemAttr(double damage, ElementAttribute attr, double adjust, double boost) 
        => attr switch
        {
            ElementAttribute.Damage0 => (1.0 - adjust) * damage,
            ElementAttribute.Damage50 => (1.0 - (adjust * 0.5 + boost)) * damage,
            ElementAttribute.Damage150 => (adjust * 0.5 + boost + 1.0) * damage,
            _ => damage
        };
}
