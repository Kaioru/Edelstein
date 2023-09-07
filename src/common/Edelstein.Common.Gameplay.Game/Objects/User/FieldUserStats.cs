using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public record struct FieldUserStats : IFieldUserStats
{
    public int Level { get; }
    
    public int STR { get; }
    public int DEX { get; }
    public int INT { get; }
    public int LUK { get; }
    
    public int MaxHP { get; }
    public int MaxMP { get; }
    
    public int PAD { get; }
    public int PDD { get; }
    public int MAD { get; }
    public int MDD { get; }
    public int ACC { get; }
    public int EVA { get; }
    
    public int Craft { get; }
    public int Speed { get; }
    public int Jump { get; }
    
    public int STRr { get; }
    public int DEXr { get; }
    public int INTr { get; }
    public int LUKr { get; }
    public int MaxHPr { get; }
    public int MaxMPr { get; }
    public int PADr { get; }
    public int PDDr { get; }
    public int MADr { get; }
    public int MDDr { get; }
    public int ACCr { get; }
    public int EVAr { get; }
    
    public int PACC { get; }
    public int MACC { get; }
    public int PEVA { get; }
    public int MEVA { get; }
    
    public int Ar { get; }
    public int Er { get; }
    
    public int Cr { get; }
    public int CDMin { get; }
    public int CDMax { get; }
    
    public int IMDr { get; }
    
    public int PDamR { get; }
    public int MDamR { get; }
    public int BossDamR { get; }
    
    public int Mastery { get; }

    public int DamageMin { get; }
    public int DamageMax { get; }
    
    public IFieldUserStatsSkillLevels SkillLevels { get; }

    public FieldUserStats(
        IFieldUser user, 
        ITemplateManager<IItemTemplate> itemTemplates,
        ITemplateManager<ISkillTemplate> skillTemplates
    )
    {
        var character = user.Character;

        Level = character.Level;
        
        STR = character.STR;
        DEX = character.DEX;
        INT = character.INT;
        LUK = character.LUK;
        MaxHP = character.MaxHP;
        MaxMP = character.MaxMP;
        
        Craft = INT + DEX + LUK;
        Speed = 100;
        Jump = 100;
        
        Cr = 5;
        CDMin = 20;
        CDMax = 50;
        
        Mastery = 0;
        
        DamageMin = 1;
        DamageMax = 1;

        SkillLevels = new FieldUserStatsSkillLevels();
        
        var equippedItems = character.Inventories[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Where(kv => kv.Value is ItemSlotEquip)
            .Select(kv => (kv.Key, (ItemSlotEquip)kv.Value))
            .ToList() ?? new List<(short Key, ItemSlotEquip)>();

        foreach (var kv in equippedItems)
        {
            var (pos, item) = kv;
            var template = itemTemplates.Retrieve(item.ID).Result;

            if (template is not IItemEquipTemplate equipTemplate) continue;
            
            STR += item.STR;
            DEX += item.DEX;
            INT += item.INT;
            LUK += item.LUK;
            MaxHP += item.MaxHP;
            MaxMP += item.MaxMP;

            if (
                pos != -(int)BodyPart.PetWear2 &&
                pos != -(int)BodyPart.PetWear3 &&
                pos != -(int)BodyPart.PetRingLabel2 &&
                pos != -(int)BodyPart.PetRingLabel3 &&
                pos != -(int)BodyPart.PetRingQuote2 &&
                pos != -(int)BodyPart.PetRingQuote3 &&
                (
                    item.ID / 10000 == 190 || 
                    pos != -(int)BodyPart.TamingMob && 
                    pos != -(int)BodyPart.Saddle && 
                    pos != -(int)BodyPart.MobEquip
                )
            )
            {
                PAD += item.PAD;
                PDD += item.PDD;
                MAD += item.MAD;
                MDD += item.MDD;
                ACC += item.ACC;
                EVA += item.EVA;
                Craft += item.Craft;
                Speed += item.Speed;
                Jump += item.Jump;
            }

            MaxHPr += equipTemplate.IncMaxHPr;
            MaxMPr += equipTemplate.IncMaxMPr;

            // TODO: and not Dragon or Mechanic
            // TODO: item options
        }

        // TODO: item sets

        foreach (var kv in user.Character.Skills.Records)
        {
            var (id, record) = kv;
            var skillTemplate = skillTemplates.Retrieve(id).Result;
            
            if (skillTemplate == null) continue;
            
            var level = kv.Value.Level;
            
            if (JobConstants.GetJobLevel(id / 10000) > 0 && id != Skill.KnightCombatOrders && level > 0)
            {
                var maxLevel = skillTemplate.MaxLevel;
                if (skillTemplate.IsCombatOrders) maxLevel += 2;

                level += character.TemporaryStats[TemporaryStatType.CombatOrders]?.Value ?? 0;
                level = Math.Min(maxLevel, level);
            }
            SkillLevels.Records[kv.Key] = level;

            var levelTemplate = skillTemplate[level];
            if (levelTemplate == null) continue;
            
            if (skillTemplate is not { IsPSD: true }) continue;
            
            // TODO: more psd handling
            
            MaxHPr += levelTemplate.MHPr;
            MaxMPr += levelTemplate.MMPr;

            Cr += levelTemplate.Cr;
            CDMin += levelTemplate.CDMin;
            CDMax += levelTemplate.CDMax;

            ACCr += levelTemplate.ACCr;
            EVAr += levelTemplate.EVAr;

            Ar += levelTemplate.Ar;
            Er += levelTemplate.Er;

            PADr += levelTemplate.PADr;
            MADr += levelTemplate.MADr;

            PAD += levelTemplate.PADx;
            MAD += levelTemplate.MADx;

            IMDr += levelTemplate.IMDr;

            Jump += levelTemplate.PsdJump;
            Speed += levelTemplate.PsdSpeed;
        }
        
        var equipped = character.Inventories[ItemInventoryType.Equip];
        var weapon = equipped != null
            ? equipped.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) 
                ? result1.ID 
                : 0
            : 0;
        var subWeapon = equipped != null
            ? equipped.Items.TryGetValue(-(short)BodyPart.Shield, out var result2) 
                ? result2.ID 
                : 0
            : 0;
        var weaponType = ItemConstants.GetWeaponType(weapon);
        var subWeaponType = ItemConstants.GetWeaponType(subWeapon);

        if (subWeapon > 0 && SkillLevels[Skill.KnightShieldMastery] > 0)
        {
            var shieldMasterySkill = skillTemplates.Retrieve(Skill.KnightShieldMastery).Result;
            var shieldMasteryLevel = shieldMasterySkill?[SkillLevels[Skill.KnightShieldMastery]];

            PDDr += shieldMasteryLevel?.X ?? 0;
            MDDr += shieldMasteryLevel?.X ?? 0;
        }
        
        STRr += character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;
        DEXr += character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;
        INTr += character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;
        LUKr += character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;

        PAD += character.TemporaryStats[TemporaryStatType.PAD]?.Value ?? 0;
        PDD += character.TemporaryStats[TemporaryStatType.PDD]?.Value ?? 0;
        MAD += character.TemporaryStats[TemporaryStatType.MAD]?.Value ?? 0;
        MDD += character.TemporaryStats[TemporaryStatType.MDD]?.Value ?? 0;
        ACC += character.TemporaryStats[TemporaryStatType.ACC]?.Value ?? 0;
        EVA += character.TemporaryStats[TemporaryStatType.EVA]?.Value ?? 0;
        Craft += character.TemporaryStats[TemporaryStatType.Craft]?.Value ?? 0;
        Speed += character.TemporaryStats[TemporaryStatType.Speed]?.Value ?? 0;
        Jump += character.TemporaryStats[TemporaryStatType.Jump]?.Value ?? 0;

        MaxHP += character.TemporaryStats[TemporaryStatType.MaxHP]?.Value ?? 0;
        MaxMP += character.TemporaryStats[TemporaryStatType.MaxMP]?.Value ?? 0;

        void GetMastery(IFieldUserStatsSkillLevels skillLevels, int skillID, ref int mastery, ref int stat)
        {
            var skillTemplate = skillTemplates.Retrieve(skillID).Result;
            var skillLevelTemplate = skillTemplate?[skillLevels[skillID]];
            if (skillLevelTemplate == null) return;

            mastery += skillLevelTemplate.Mastery;
            stat += skillLevelTemplate.X;
        }

        var incMastery = 0;
        var incNone = 0;
        var incACC = 0;
        
        switch (weaponType)
        {
            case WeaponType.OneHandedSword:
            case WeaponType.TwoHandedSword:
                {
                    foreach (var skill in new List<int>
                             {
                                 Skill.FighterWeaponMastery,
                                 Skill.PageWeaponMastery,
                                 Skill.SoulmasterSwordMastery
                             }.TakeWhile(_ => incMastery == 0))
                    {
                        GetMastery(SkillLevels, skill, ref incMastery, ref incACC);

                        if (skill != Skill.PageWeaponMastery ||
                            incMastery <= 0 ||
                            character.TemporaryStats[TemporaryStatType.WeaponCharge] == null) continue;
                        
                        var incMastery2 = 0;
                            
                        GetMastery(SkillLevels, Skill.PaladinAdvancedCharge, ref incMastery2, ref incNone);
                        if (incMastery2 > 0)
                            incMastery = incMastery2;
                    }
                    break;
                }
            case WeaponType.OneHandedAxe:
            case WeaponType.TwoHandedAxe:
                GetMastery(SkillLevels, Skill.FighterWeaponMastery, ref incMastery, ref incACC);
                break;
            case WeaponType.OneHandedMace:
            case WeaponType.TwoHandedMace:
            {
                GetMastery(SkillLevels, Skill.PageWeaponMastery, ref incMastery, ref incACC);
                
                if (incMastery > 0 && character.TemporaryStats[TemporaryStatType.WeaponCharge] != null)
                {
                    var incMastery2 = 0;
                            
                    GetMastery(SkillLevels, Skill.PaladinAdvancedCharge, ref incMastery2, ref incNone);
                    if (incMastery2 > 0)
                        incMastery = incMastery2;
                }
                break;
            }
            case WeaponType.Spear:
            case WeaponType.Polearm:
            {
                GetMastery(SkillLevels, Skill.SpearmanWeaponMastery, ref incMastery, ref incACC);
                break;
            }
        }

        Mastery += incMastery;
        ACC += incACC;

        STR += (int)(STR * (STRr / 100d));
        DEX += (int)(DEX * (DEXr / 100d));
        INT += (int)(INT * (INTr / 100d));
        LUK += (int)(LUK * (LUKr / 100d));
        MaxHP += (int)(MaxHP * (MaxHPr / 100d));
        MaxMP += (int)(MaxMP * (MaxMPr / 100d));
        
        PAD += (int)(PAD * (PADr / 100d));
        PDD += (int)(STR * 1.2 + LUK * 0.5 + DEX * 0.5 + INT * 0.4);
        PDD += (int)(PDD * (PDDr / 100d));
        MAD += (int)(MAD * (MADr / 100d));
        MDD += (int)(INT * 1.2 + DEX * 0.5 + LUK * 0.5 + STR * 0.4);
        MDD += (int)(MDD * (MDDr / 100d));
        
        PACC = (int)(DEX * 1.2 + LUK) + ACC;
        PACC += (int)(PACC * (ACCr / 100d));
        MACC = (int)(LUK * 1.2 + INT) + ACC;
        MACC += (int)(MACC * (ACCr / 100d));
        PEVA = LUK * 2 + DEX + EVA;
        PEVA += (int)(PEVA * (EVAr / 100d));
        MEVA = LUK * 2 + INT + EVA;
        MEVA += (int)(MEVA * (EVAr / 100d));
        
        MaxHP = Math.Min(MaxHP, 99999);
        MaxMP = Math.Min(MaxMP, 99999);

        PAD = Math.Min(PAD, 29999);
        PDD = Math.Min(PDD, 30000);
        MAD = Math.Min(MAD, 29999);
        MDD = Math.Min(MDD, 30000);
        PACC = Math.Min(PACC, 9999);
        MACC = Math.Min(MACC, 9999);
        PEVA = Math.Min(PEVA, 9999);
        MEVA = Math.Min(MEVA, 9999);
        Speed = Math.Min(Math.Max(Speed, 100), 140);
        Jump = Math.Min(Math.Max(Jump, 100), 123);
        
        CDMin = Math.Min(CDMin, CDMax);

        var stat1 = 0;
        var stat2 = 0;
        var stat3 = 0;
        var attack = PAD;
        var multiplier = 0.0;
        
        if (JobConstants.GetJobLevel(character.Job) == 0)
        {
            stat1 = STR;
            stat2 = DEX;
            multiplier = 1.2;
        }
        else if (JobConstants.GetJobType(character.Job) == JobType.Magician)
        {
            stat1 = INT;
            stat2 = LUK;
            attack = MAD;
            multiplier = 1.0;
        }
        else
        {
            switch (weaponType)
            {
                case WeaponType.OneHandedSword:
                case WeaponType.OneHandedAxe:
                case WeaponType.OneHandedMace:
                    stat1 = STR;
                    stat2 = DEX;
                    multiplier = 1.20;
                    break;
                case WeaponType.TwoHandedSword:
                case WeaponType.TwoHandedAxe:
                case WeaponType.TwoHandedMace:
                    stat1 = STR;
                    stat2 = DEX;
                    multiplier = 1.32;
                    break;
                case WeaponType.Dagger:
                    stat1 = LUK;
                    stat2 = DEX;
                    stat3 = STR;
                    multiplier = 1.30;
                    break;
                case WeaponType.Barehand:
                    stat1 = STR;
                    stat2 = DEX;
                    attack = 1;
                    multiplier = 1.43;
                    break;
                case WeaponType.Polearm:
                case WeaponType.Spear:
                    stat1 = STR;
                    stat2 = DEX;
                    multiplier = 1.49;
                    break;
                case WeaponType.Bow:
                    stat1 = DEX;
                    stat2 = STR;
                    multiplier = 1.20;
                    break;
                case WeaponType.Crossbow:
                    stat1 = DEX;
                    stat2 = STR;
                    multiplier = 1.35;
                    break;
                case WeaponType.ThrowingGlove:
                    stat1 = LUK;
                    stat2 = DEX;
                    multiplier = 1.75;
                    break;
                case WeaponType.Knuckle:
                    stat1 = STR;
                    stat2 = DEX;
                    multiplier = 1.70;
                    break;
                case WeaponType.Gun:
                    stat1 = DEX;
                    stat2 = STR;
                    multiplier = 1.50;
                    break;
            }
        }
        
        var masteryConst = weaponType switch
        {
            WeaponType.Wand or
            WeaponType.Staff => 0.25,
            WeaponType.Bow or
            WeaponType.Crossbow or
            WeaponType.ThrowingGlove or
            WeaponType.Gun => 0.15,
            _ => 0.20,
        };
        var masteryMultiplier = Mastery / 100d;

        masteryMultiplier += masteryConst;
        masteryMultiplier = Math.Min(0.95, masteryMultiplier);

        Console.WriteLine(masteryMultiplier);
        DamageMax = (int)((stat3 + stat2 + 4 * stat1) / 100d * attack * multiplier + 0.5);
        DamageMin = (int)(masteryMultiplier * DamageMax + 0.5);

        DamageMin = Math.Min(DamageMin, DamageMax);
        DamageMin = Math.Min(Math.Max(DamageMin, 1), 999999);
        DamageMax = Math.Min(Math.Max(DamageMax, 1), 999999);
    }
}
