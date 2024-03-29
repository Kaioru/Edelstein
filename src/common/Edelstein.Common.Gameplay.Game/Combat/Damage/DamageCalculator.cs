﻿using Edelstein.Common.Constants;
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
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
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
        var s1 = (uint)random.Next(int.MinValue, int.MaxValue);
        var s2 = (uint)random.Next(int.MinValue, int.MaxValue);
        var s3 = (uint)random.Next(int.MinValue, int.MaxValue);
        
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
    
    public async Task<IDamage[]> CalculatePDamage(
        ICharacter character, 
        IFieldUserStats stats, 
        IFieldMob mob,
        IFieldMobStats mobStats,
        IAttack attack,
        IAttackMobEntry attackMob
    )
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        
        _rndGenForCharacter.Next(random.Array);
        
        var skill = attack.SkillID > 0 ? await _skills.Retrieve(attack.SkillID) : null;
        var skillActingID = attack.SkillID;

        if (skillActingID is Skill.AranFullSwingTs or Skill.AranFullSwingDs)
            skillActingID = Skill.AranFullSwing;
        if (skillActingID is Skill.AranOverSwingTs or Skill.AranOverSwingDs)
            skillActingID = Skill.AranOverSwing;
        if (skillActingID is Skill.Dual3HustleRush)
            skillActingID = Skill.Dual3HustleDash;
        if (skillActingID is Skill.MechanicFlamethrowerUp)
            skillActingID = Skill.MechanicWeaponmastery;
        if (skillActingID is Skill.MechanicGatlingUp)
            skillActingID = Skill.MechanicWeaponmastery;
        
        var skillLevel = skill?[stats.SkillLevels[skillActingID]];
        var equipped = character.Inventories[ItemInventoryType.Equip];
        var weapon = equipped != null
            ? equipped.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) 
                ? result1.ID 
                : 0
            : 0;
        var weaponType = ItemConstants.GetWeaponType(weapon);
        var damagePerMob = attack.Type == AttackType.Shoot 
            ? Math.Max(skillLevel?.BulletCount ?? 1, skillLevel?.AttackCount ?? 1)
            : skillLevel?.AttackCount ?? 1;

        damagePerMob = Math.Max((short)1, damagePerMob);

        if (attack is { SkillID: 0, AttackActionType: AttackActionType.DualDagger })
            damagePerMob = 2;
        
        switch (attack.SkillID)
        {
            case Skill.SniperStrafe:
                var ultimateStrafeSkill = await _skills.Retrieve(Skill.CrossbowmasterUltimateStrafe);
                var ultimateStrafeLevel = ultimateStrafeSkill?[stats.SkillLevels[Skill.CrossbowmasterUltimateStrafe]];

                damagePerMob = ultimateStrafeLevel?.BulletCount ?? damagePerMob;
                break;
        }

        var darkForceSkill = await _skills.Retrieve(Skill.DarkknightDarkForce);
        var darkForceLevel = darkForceSkill?[stats.SkillLevels[Skill.DarkknightDarkForce]];
        var isDarkForce = 
            darkForceLevel != null && 
            character.Job == Job.DarkKnight &&
            character.HP >= stats.MaxHP * darkForceLevel.X / 100;

        if (isDarkForce && attack.SkillID == Skill.DragonknightDragonBurster)
            damagePerMob += darkForceLevel?.Y ?? 0;

        var damageCalcShadowPartner = new int[damagePerMob];
        var damagePerMobShadowPartner = damagePerMob;

        if (attack.IsShadowPartner &&
            attack.SkillID != Skill.Dual3FlashBang &&
            attack.SkillID != Skill.Dual4OwlDeath)
            damagePerMobShadowPartner = (short)Math.Min(15, damagePerMobShadowPartner * 2);
        
        var result = new IDamage[damagePerMobShadowPartner];
        
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

        for (var i = 0; i < result.Length; i++)
        {
            random.Skip(); // CalcPImmune

            if (hitRate < GetRandomInRange(random.Next(), 0, 100))
            {
                result[i] = new Damage(0);
                continue;
            }
            
            if (skillLevel?.FixDamage > 0)
            {
                result[i] = new Damage(skillLevel.FixDamage);
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
            
            if (i >= damagePerMob)
            {
                var shadowPartnerSkillID = character.TemporaryStats[TemporaryStatType.ShadowPartner]?.Reason;
                var shadowPartnerSkill = await _skills.Retrieve(shadowPartnerSkillID ?? 0);
                var shadowPartnerLevel = shadowPartnerSkill?[stats.SkillLevels[shadowPartnerSkillID ?? 0]];
                
                result[i] = new Damage(damageCalcShadowPartner[i - damagePerMob] * (shadowPartnerLevel?.X ?? 0) / 100);
                continue;
            }
            
            var damage = (double)stats.DamageMax;
            var critical = false;
            var isLuckySevenOrTripleThrow = false;

            switch (attack.SkillID)
            {
                case Skill.RogueLuckySeven or Skill.NightwalkerLuckySeven:
                    isLuckySevenOrTripleThrow = true;
                    damage = GetRandomInRange(random.Next(), stats.LUK, stats.LUK * 1.4) * 5.5 * stats.PAD / 100d;
                    break;
                case Skill.NightlordTripleThrow or Skill.NightwalkerTripleThrow:
                    isLuckySevenOrTripleThrow = true;
                    damage = GetRandomInRange(random.Next(), stats.LUK, stats.LUK * 1.4) * 6.0 * stats.PAD / 100d;
                    break;
            }

            if (weaponType is WeaponType.Wand or WeaponType.Staff)
                damage *= 0.2;

            if (!SkillConstants.IsShootAction(attack.AttackAction) &&
                !SkillConstants.IsJaguarMeleeAttackSkill(attack.AttackAction) &&
                attack.SkillID != Skill.WildhunterFlashRain &&
                character.TemporaryStats.RideVehicleRecord?.Value != 1932016)
            {
                switch (weaponType)
                {
                    case WeaponType.Bow or WeaponType.Crossbow:
                        damage *= 0.6;
                        break;
                    case WeaponType.ThrowingGlove when
                        attack.SkillID != Skill.NightlordNinjaStorm &&
                        attack.SkillID != Skill.ShadowerAssassination &&
                        attack.SkillID != Skill.NightwalkerVampire &&
                        attack.SkillID != Skill.NightwalkerPoisonBomb:
                        damage *= 0.4;
                        break;
                    case WeaponType.Gun when
                        attack.AttackAction != 118 &&
                        attack.AttackAction != 119 &&
                        attack.SkillID != Skill.GunslingerThrowingBomb &&
                        attack.SkillID != Skill.CaptainAirStrike &&
                        attack.SkillID != Skill.CaptainBattleshipCannon &&
                        attack.SkillID != Skill.CaptainBattleshipTorpedo &&
                        attack.SkillID != Skill.Dual5MonsterBomb:
                    {
                        damage *= 0.4;
                    
                        if (attack.AttackAction is 102 or 101)
                            damage *= 1.8;
                        break;
                    }
                }
            }

            if (SkillConstants.IsProneStabAction(attack.AttackAction))
                damage *= 0.1;
            
            if (!isLuckySevenOrTripleThrow)
                damage = GetDamageAdjustedByRandom(damage, stats.Mastery, ItemConstants.GetMasteryConstByWeaponType(weaponType), random.Next());

            if (mobStats.Level > stats.Level)
                damage *= (100d - (mobStats.Level - stats.Level)) / 100d;

            damage += damage * stats.PDamR / 100d;

            // ElemBoost
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
                ? skillLevel?.Damage != 0 ? skillLevel?.Damage ?? 100d : 100d
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
                
                case Skill.ArcherArrowBlow:
                case Skill.ArcherDoubleShot:
                {
                    foreach (var skillID in new List<int>
                             {
                                 Skill.HunterImproveBasic,
                                 Skill.CrossbowmanImproveBasic,
                             }.Where(skillID => stats.SkillLevels[skillID] > 0))
                    {
                        var archerImproveBasicSkill = await _skills.Retrieve(skillID);
                        var archerImproveBasicLevel = archerImproveBasicSkill?[stats.SkillLevels[skillID]];

                        if (archerImproveBasicLevel == null) break;

                        skillDamageR += attack.SkillID == Skill.ArcherArrowBlow
                            ? archerImproveBasicLevel.X
                            : archerImproveBasicLevel.Y;
                        break;
                    }

                    break;
                }
                case Skill.SniperStrafe:
                    var ultimateStrafeSkill = await _skills.Retrieve(Skill.CrossbowmasterUltimateStrafe);
                    var ultimateStrafeLevel = ultimateStrafeSkill?[stats.SkillLevels[Skill.CrossbowmasterUltimateStrafe]];
                    
                    skillDamageR = ultimateStrafeLevel?.Damage ?? skillDamageR;
                    break;
                case Skill.WildhunterFlashRain:
                    if (i == damagePerMob - 1)
                    {
                        var flashRainSkill = await _skills.Retrieve(Skill.WildhunterFlashRain);
                        var flashRainLevel = flashRainSkill?[stats.SkillLevels[Skill.WildhunterFlashRain]];

                        skillDamageR = flashRainLevel?.X ?? skillDamageR;
                    }
                    break;
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

            if (attack.IsShadowPartner)
                damageCalcShadowPartner[i] = (int)damage;

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
            result[i] = new Damage((int)damage, critical);
        }
        
        return result;
    }
    
    public async Task<IDamage[]> CalculateMDamage(
        ICharacter character, 
        IFieldUserStats stats,
        IFieldMob mob,
        IFieldMobStats mobStats, 
        IAttack attack,
        IAttackMobEntry attackMob
    )    
    {
        var random = new Rotational<uint>(new uint[RndSize]);
        var skill = attack.SkillID > 0 ? await _skills.Retrieve(attack.SkillID) : null;
        var skillActingID = attack.SkillID;
        
        if (skillActingID is 
            Skill.BmageFinishAttack or 
            Skill.BmageFinishAttack1 or 
            Skill.BmageFinishAttack2 or 
            Skill.BmageFinishAttack3 or 
            Skill.BmageFinishAttack4 or 
            Skill.BmageFinishAttack5)
            skillActingID = Skill.BmageFinishAttack;
        
        var skillLevel = skill?[stats.SkillLevels[skillActingID]];
        var damagePerMob = skillLevel?.AttackCount ?? 1;
        var result = new IDamage[damagePerMob];

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

        for (var i = 0; i < damagePerMob; i++)
        {
            random.Skip(); // CalcMImmune

            if (hitRate < GetRandomInRange(random.Next(), 0, 100))
            {
                result[i] = new Damage(0);
                continue;
            }
            
            if (skillLevel?.FixDamage > 0)
            {
                result[i] = new Damage(skillLevel.FixDamage);
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
            result[i] = new Damage((int)damage, critical);
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
    
    public async Task<int[]> CalculateAdjustedDamage(
        ICharacter character, 
        IFieldUserStats stats,
        IFieldMob mob,
        IFieldMobStats mobStats, 
        IAttack attack,
        IDamage[] damage, 
        int mobCount,
        int mobOrder
    )
    {
        var result = new int[damage.Length];
        var rate = 1d;

        if (attack.IsFinalAfterSlashBlast)
            rate = Math.Pow(1 / 3d, mobOrder);

        if (attack.SkillID is Skill.Archmage2ChainLightning or Skill.EvanBlaze)
        {
            var damageDecSkill = await _skills.Retrieve(attack.SkillID);
            var damageDecLevel = damageDecSkill?[stats.SkillLevels[attack.SkillID]];

            rate = (100 - mobOrder * (damageDecLevel?.X ?? 0)) / 100d;
        }

        for (var i = 0; i < damage.Length; i++)
            result[i] = (int)(damage[i].Value * rate);
        
        if (attack.SkillID is 0 or
                Skill.LegendFinalBlow or
                Skill.AranDoubleSwing or
                Skill.AranFinalToss or
                Skill.AranFinalBlow or
                Skill.AranOverSwingDs or
                Skill.AranOverSwingTs or
                Skill.AranRollingSpin or
                Skill.AranFullSwingDs or
                Skill.AranFullSwingTs &&
            mobCount > 1 &&
            (
                mob.TemporaryStats[MobTemporaryStatType.Freeze] == null || 
                mob.TemporaryStats[MobTemporaryStatType.Freeze]?.Reason != Skill.AranComboTempest
            )
        )
        {
            for (var i = 0; i < damage.Length; i++)
                result[i] = (int)(((stats.Level / 10.0 + 20.0) * (mobCount - 1) / 100.0 + 1.0) / mobCount * result[i]);
        }
        
        if (attack.AttackActionType == AttackActionType.DualDagger && 
            (attack.IsShadowPartner 
                ? damage.Length / 2 
                : damage.Length
            ) > 1 &&
            damage[0].Value != mob.Template.MaxHP && 
            damage[0].Value != 999999 &&
            (attack.SkillID == 0 || 
             attack.SkillID / 100000 == 43 || 
             attack.SkillID / 10000 == 900
            ) && 
            (character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Weapon, out var itemDagger) ?? false) && 
            (character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Shield, out var itemDual) ?? false) &&
            itemDagger is IItemSlotEquip equipDagger &&
            itemDual is IItemSlotEquip equipDual &&
            equipDagger.PAD + equipDual.PAD > 0
        )
        {
            var rateDagger = (double)equipDagger.PAD / (equipDagger.PAD + equipDual.PAD);
            var rateDual = 1.0 - rateDagger;
            
            for (var i = 0; i < damage.Length; i++)
                result[i] = i % 2 == 0
                    ? (int)(result[i] * rateDagger)
                    : (int)(result[i] * rateDual);
        }
        
        return result;
    }

    private static double GetRandomInRange(uint rand, double f0, double f1)
    {
        if (Math.Abs(f0 - f1) < 0.0001) return f0;
        if (f0 > f1) (f0, f1) = (f1, f0);
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
