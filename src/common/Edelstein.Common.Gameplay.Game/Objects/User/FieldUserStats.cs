using System.Collections.Immutable;
using Edelstein.Common.Constants;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.User;

public record FieldUserStats : IFieldUserStats
{
    public FieldUserStats()
    {
        SkillLevels = new FieldUserStatsSkillLevels();
        Reset();
    }
    
    public int Level { get; private set; }
    
    public int STR { get; private set; }
    public int DEX { get; private set; }
    public int INT { get; private set; }
    public int LUK { get; private set; }
    
    public int MaxHP { get; private set; }
    public int MaxMP { get; private set; }
    
    public int PAD { get; private set; }
    public int PDD { get; private set; }
    public int MAD { get; private set; }
    public int MDD { get; private set; }
    public int ACC { get; private set; }
    public int EVA { get; private set; }
    
    public int Craft { get; private set; }
    public int Speed { get; private set; }
    public int Jump { get; private set; }
    
    public int STRr { get; private set; }
    public int DEXr { get; private set; }
    public int INTr { get; private set; }
    public int LUKr { get; private set; }
    public int MaxHPr { get; private set; }
    public int MaxMPr { get; private set; }
    public int PADr { get; private set; }
    public int PDDr { get; private set; }
    public int MADr { get; private set; }
    public int MDDr { get; private set; }
    public int ACCr { get; private set; }
    public int EVAr { get; private set; }
    
    public int PACC { get; private set; }
    public int MACC { get; private set; }
    public int PEVA { get; private set; }
    public int MEVA { get; private set; }
    
    public int Ar { get; private set; }
    public int Er { get; private set; }
    
    public int Cr { get; private set; }
    public int CDMin { get; private set; }
    public int CDMax { get; private set; }
    
    public int IMDr { get; private set; }
    
    public int PDamR { get; private set; }
    public int MDamR { get; private set; }
    public int BossDamR { get; private set; }
    
    public int Mastery { get; private set; }
    
    public int AttackSpeedBase { get; private set; }
    public int AttackSpeed { get; private set; }

    public int DamageMin { get; private set; }
    public int DamageMax { get; private set; }
    
    public int CompletedSetItemID { get; private set; }

    public IFieldUserStatsSkillLevels SkillLevels { get; }
    
    public async Task Apply(IFieldUser user)
    {
        Reset();
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
        
        AttackSpeedBase = 6;
        AttackSpeed = 6;
        
        DamageMin = 1;
        DamageMax = 1;

        CompletedSetItemID = 0;

        SkillLevels.Records.Clear();
        
        await ApplyItems(user);
        await ApplySkills(user);
        await ApplyTemporaryStats(user);
        await ApplyMastery(user);
        
        STR += (int)(STR * (STRr / 100d));
        DEX += (int)(DEX * (DEXr / 100d));
        INT += (int)(INT * (INTr / 100d));
        LUK += (int)(LUK * (LUKr / 100d));
        MaxHP += (int)(MaxHP * (MaxHPr / 100d));
        MaxMP += (int)(MaxMP * (MaxMPr / 100d));

        STR = user.StatsForced.STR ?? STR;
        DEX = user.StatsForced.DEX ?? DEX;
        INT = user.StatsForced.INT ?? INT;
        LUK = user.StatsForced.LUK ?? LUK;
        
        PAD += (int)(PAD * (PADr / 100d));
        PAD = user.StatsForced.PAD ?? PAD;
        
        PDD += (int)(PDD * (PDDr / 100d));
        PDD = user.StatsForced.PDD ?? PDD;
        PDD += (int)(STR * 1.2 + LUK * 0.5 + DEX * 0.5 + INT * 0.4);
        
        MAD += (int)(MAD * (MADr / 100d));
        MAD = user.StatsForced.MAD ?? MAD;
        
        MDD += (int)(MDD * (MDDr / 100d));
        MDD = user.StatsForced.MDD ?? MDD;
        MDD += (int)(INT * 1.2 + DEX * 0.5 + LUK * 0.5 + STR * 0.4);
        
        ACC = user.StatsForced.ACC ?? ACC;
        EVA = user.StatsForced.EVA ?? EVA;
        
        PACC = (int)(DEX * 1.2 + LUK) + ACC;
        PACC += (int)(PACC * (ACCr / 100d));
        MACC = (int)(LUK * 1.2 + INT) + ACC;
        MACC += (int)(MACC * (ACCr / 100d));
        PEVA = LUK * 2 + DEX + EVA;
        PEVA += (int)(PEVA * (EVAr / 100d));
        MEVA = LUK * 2 + INT + EVA;
        MEVA += (int)(MEVA * (EVAr / 100d));

        Speed = user.StatsForced.Speed ?? Speed;
        Jump = user.StatsForced.Jump ?? Jump;
        
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
        
        Speed = Math.Min(Math.Max(Speed, 100), user.StatsForced.SpeedMax ?? 140);
        Jump = Math.Min(Math.Max(Jump, 100), 123);
        
        CDMin = Math.Min(CDMin, CDMax);
        
        AttackSpeedBase = Math.Min(Math.Max(AttackSpeedBase, 2), 10);
        AttackSpeed = Math.Min(Math.Max(AttackSpeed, 2), 10);
        
        await ApplyDamage(user);
        
        DamageMin = Math.Min(Math.Max(DamageMin, 1), 999999);
        DamageMax = Math.Min(Math.Max(DamageMax, 1), 999999);
    }
    
    public void Reset()
    {    
        Level = 0;
        
        STR = 0;
        DEX = 0;
        INT = 0;
        LUK = 0;
        
        MaxHP = 0;
        MaxMP = 0;
        
        PAD = 0;
        PDD = 0;
        MAD = 0;
        MDD = 0;
        ACC = 0;
        EVA = 0;
        
        Craft = 0;
        Speed = 0;
        Jump = 0;
        
        STRr = 0;
        DEXr = 0;
        INTr = 0;
        LUKr = 0;
        MaxHPr = 0;
        MaxMPr = 0;
        PADr = 0;
        PDDr = 0;
        MADr = 0;
        MDDr = 0;
        ACCr = 0;
        EVAr = 0;
        
        PACC = 0;
        MACC = 0;
        PEVA = 0;
        MEVA = 0;
        
        Ar = 0;
        Er = 0;
        
        Cr = 0;
        CDMin = 0;
        CDMax = 0;
        
        IMDr = 0;
        
        PDamR = 0;
        MDamR = 0;
        BossDamR = 0;
        
        Mastery = 0;

        DamageMin = 0;
        DamageMax = 0;
    }

    private async Task ApplyItems(IFieldUser user)
    {
        var equipped = user.Character.Inventories[ItemInventoryType.Equip]?.Items
            .Where(kv => kv.Key < 0)
            .Where(kv => kv.Value is ItemSlotEquip)
            .Select(kv => (kv.Key, (ItemSlotEquip)kv.Value))
            .ToImmutableArray() ?? ImmutableArray<(short Key, ItemSlotEquip)>.Empty;
        var setCompletion = new Dictionary<int, int>();

        foreach (var (slot, item) in equipped)
        {
            var template = await user.StageUser.Context.Templates.Item.Retrieve(item.ID);

            if (template is not IItemEquipTemplate equip) continue;
            
            STR += item.STR;
            DEX += item.DEX;
            INT += item.INT;
            LUK += item.LUK;
            MaxHP += item.MaxHP;
            MaxMP += item.MaxMP;

            if (slot != -(int)BodyPart.PetWear2 &&
                slot != -(int)BodyPart.PetWear3 &&
                slot != -(int)BodyPart.PetRingLabel2 &&
                slot != -(int)BodyPart.PetRingLabel3 &&
                slot != -(int)BodyPart.PetRingQuote2 &&
                slot != -(int)BodyPart.PetRingQuote3 &&
                (
                    item.ID / 10000 == 190 || 
                    slot != -(int)BodyPart.TamingMob && 
                    slot != -(int)BodyPart.Saddle && 
                    slot != -(int)BodyPart.MobEquip
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

                MaxHPr += equip.IncMaxHPr;
                MaxMPr += equip.IncMaxMPr;

                if (item.Grade is
                    ItemGrade.Rare or
                    ItemGrade.Epic or
                    ItemGrade.Unique)
                {
                    var level = (equip.ReqLevel - 1) / 10;

                    level = Math.Max(1, level);
                    level = Math.Min(20, level);
                
                    await ApplyItemOption(user, item.Option1, level);
                    await ApplyItemOption(user, item.Option2, level);
                    await ApplyItemOption(user, item.Option3, level);
                }
            }

            if (equip.SetItemID > 0)
                setCompletion[equip.SetItemID] = (setCompletion.TryGetValue(equip.SetItemID, out var count) ? count : 0) + 1;

            if (slot == -(short)BodyPart.Weapon)
            {
                AttackSpeedBase = equip.AttackSpeed ?? AttackSpeed;
                AttackSpeed = equip.AttackSpeed ?? AttackSpeed;
            }
        }
        
        foreach (var (setItemID, count) in setCompletion)
        {
            var setItemTemplate = await user.StageUser.Context.Templates.ItemSet.Retrieve(setItemID);
            if (setItemTemplate == null) continue;

            for (var i = 1; i <= count; i++)
            {
                var setItemEffect = setItemTemplate.Effects.TryGetValue(i, out var effect) ? effect : null;
                if (setItemEffect == null) continue;
                STR += setItemEffect.IncSTR;
                DEX += setItemEffect.IncDEX;
                INT += setItemEffect.IncINT;
                LUK += setItemEffect.IncLUK;
                
                MaxHP += setItemEffect.IncMaxHP;
                MaxMP += setItemEffect.IncMaxMP;
                
                PAD += setItemEffect.IncPAD;
                MAD += setItemEffect.IncMAD;
                PDD += setItemEffect.IncPDD;
                MDD += setItemEffect.IncMDD;
                
                ACC += setItemEffect.IncACC;
                EVA += setItemEffect.IncEVA;
                
                Craft += setItemEffect.IncCraft;
                Speed += setItemEffect.IncSpeed;
                Jump += setItemEffect.IncJump;
            }

            if (count == setItemTemplate.SetCompleteCount)
                CompletedSetItemID = setItemID;
        }

        var weaponType = ItemConstants.GetWeaponType(
            user.Character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) ?? false
                ? result1.ID 
                : 0);
        var rechargeable = user.Character.Inventories[ItemInventoryType.Consume]?.Items.Values
                .OfType<IItemSlotBundle>()
                .FirstOrDefault(
                    i => 
                        (weaponType == WeaponType.ThrowingGlove && i.ID / 10000 == 207 || 
                         weaponType == WeaponType.Gun && i.ID / 10000 == 233) &&
                        i.Number > 0);

        if (rechargeable != null)
        {
            var template = await user.StageUser.Context.Templates.Item.Retrieve(rechargeable.ID);
            if (template is IItemBundleTemplate bundle && user.Character.Level > bundle.ReqLevel)
                PAD += bundle.IncPAD;
        }
    }

    private async Task ApplyItemOption(IFieldUser user, int option, int level)
    {
        var template = await user.StageUser.Context.Templates.ItemOption.Retrieve(option);
        if (template == null) return;
        if (!template.Levels.TryGetValue(level, out var levelTemplate)) return;
        
        STR += levelTemplate.IncSTR;
        DEX += levelTemplate.IncDEX;
        LUK += levelTemplate.IncLUK;
        INT += levelTemplate.IncINT;

        MaxHP += levelTemplate.IncMaxHP;
        MaxMP += levelTemplate.IncMaxMP;

        PAD += levelTemplate.IncPAD;
        PDD += levelTemplate.IncPDD;
        MAD += levelTemplate.IncMAD;
        MDD += levelTemplate.IncMDD;
        ACC += levelTemplate.IncACC;
        EVA += levelTemplate.IncEVA;

        Speed += levelTemplate.IncSpeed;
        Jump += levelTemplate.IncJump;
    
        STRr += levelTemplate.IncSTRr;
        DEXr += levelTemplate.IncDEXr;
        LUKr += levelTemplate.IncLUKr;
        INTr += levelTemplate.IncINTr;
    
        MaxHPr += levelTemplate.IncMaxHPr;
        MaxMPr += levelTemplate.IncMaxMPr;
    
        PADr += levelTemplate.IncPADr;
        PDDr += levelTemplate.IncPDDr;
        MADr += levelTemplate.IncMADr;
        MDDr += levelTemplate.IncMDDr;
        ACCr += levelTemplate.IncACCr;
        EVAr += levelTemplate.IncEVAr;
    }
    
    private async Task ApplySkills(IFieldUser user)
    {
        var subWeapon = 
            user.Character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Shield, out var result2) ?? false
                ? result2.ID
                : 0;
        foreach (var (skillID, record) in user.Character.Skills.Records)
        {
            var skillLevel = record.Level;
            var template = await user.StageUser.Context.Templates.Skill.Retrieve(skillID);
            if (template == null) continue;
            
            if (JobConstants.GetJobLevel(skillID / 10000) > 0 && skillID != Skill.KnightCombatOrders && skillLevel > 0)
            {
                var maxLevel = template.MaxLevel;
                
                if (template.IsCombatOrders) maxLevel += 2;

                skillLevel += user.Character.TemporaryStats[TemporaryStatType.CombatOrders]?.Value ?? 0;
                skillLevel = Math.Min(maxLevel, skillLevel);
            }
            
            SkillLevels.Records[skillID] = skillLevel;

            var levelTemplate = template[skillLevel];
            if (levelTemplate == null) continue;
            if (template is not { IsPSD: true }) continue;
            
            MaxHPr += levelTemplate.MHPr;
            MaxMPr += levelTemplate.MMPr;

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
            
            if (template.PsdSkill.Count > 0) continue;
            
            Cr += levelTemplate.Cr;
            CDMin += levelTemplate.CDMin;
            CDMax += levelTemplate.CDMax;
        }
        
        if (JobConstants.GetJobRace(user.Character.Job) == 0 &&
            JobConstants.GetJobType(user.Character.Job) == 1 && 
            JobConstants.GetJobBranch(user.Character.Job) == 2 &&
            subWeapon > 0)
        {
            var shieldMasterySkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.KnightShieldMastery);
            var shieldMasteryLevel = shieldMasterySkill?[SkillLevels[Skill.KnightShieldMastery]];

            PDDr += shieldMasteryLevel?.X ?? 0;
            MDDr += shieldMasteryLevel?.X ?? 0;
        }
        
        if (JobConstants.GetJobRace(user.Character.Job) == 0 &&
            JobConstants.GetJobType(user.Character.Job) == 3)
        {
            var criticalShotSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.ArcherCriticalShot);
            var criticalShotLevel = criticalShotSkill?[SkillLevels[Skill.ArcherCriticalShot]];

            Cr += criticalShotLevel?.Prop ?? 0;
        }
        
        if (JobConstants.GetJobRace(user.Character.Job) == 0 &&
            JobConstants.GetJobType(user.Character.Job) == 4)
        {
            var nimbleBodySkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.RogueNimbleBody);
            var nimbleBodyLevel = nimbleBodySkill?[SkillLevels[Skill.RogueNimbleBody]];

            ACC += nimbleBodyLevel?.X ?? 0;
            EVA += nimbleBodyLevel?.Y ?? 0;
        }
        
        if (JobConstants.GetJobRace(user.Character.Job) == 0 &&
            JobConstants.GetJobType(user.Character.Job) == 5)
        {
            var quickMotionSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.PirateQuickmotion);
            var quickMotionLevel = quickMotionSkill?[SkillLevels[Skill.PirateQuickmotion]];

            ACC += quickMotionLevel?.X ?? 0;
            EVA += quickMotionLevel?.Y ?? 0;
        }
        
        if (JobConstants.GetJobRace(user.Character.Job) == 2 &&
            JobConstants.GetJobType(user.Character.Job) == 2)
        {
            var dragonSoulSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.EvanDragonSoul);
            var dragonSoulLevel = dragonSoulSkill?[SkillLevels[Skill.EvanDragonSoul]];

            MAD += dragonSoulLevel?.MAD ?? 0;
            
            var criticalMagicSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.EvanMagicCritical);
            var criticalMagicLevel = criticalMagicSkill?[SkillLevels[Skill.EvanMagicCritical]];

            Cr += criticalMagicLevel?.Prop ?? 0;
        }
    }
    
    private async Task ApplyTemporaryStats(IFieldUser user)
    {
        STRr += user.Character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;
        DEXr += user.Character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;
        INTr += user.Character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;
        LUKr += user.Character.TemporaryStats[TemporaryStatType.BasicStatUp]?.Value ?? 0;

        PAD += user.Character.TemporaryStats[TemporaryStatType.PAD]?.Value ?? 0;
        PDD += user.Character.TemporaryStats[TemporaryStatType.PDD]?.Value ?? 0;
        MAD += user.Character.TemporaryStats[TemporaryStatType.MAD]?.Value ?? 0;
        MDD += user.Character.TemporaryStats[TemporaryStatType.MDD]?.Value ?? 0;
        
        PAD += user.Character.TemporaryStats[TemporaryStatType.EPAD]?.Value ?? 0;
        PDD += user.Character.TemporaryStats[TemporaryStatType.EPDD]?.Value ?? 0;
        MDD += user.Character.TemporaryStats[TemporaryStatType.EMDD]?.Value ?? 0;
        
        ACC += user.Character.TemporaryStats[TemporaryStatType.ACC]?.Value ?? 0;
        EVA += user.Character.TemporaryStats[TemporaryStatType.EVA]?.Value ?? 0;
        Craft += user.Character.TemporaryStats[TemporaryStatType.Craft]?.Value ?? 0;
        Speed += user.Character.TemporaryStats[TemporaryStatType.Speed]?.Value ?? 0;
        Jump += user.Character.TemporaryStats[TemporaryStatType.Jump]?.Value ?? 0;

        MaxHPr += user.Character.TemporaryStats[TemporaryStatType.MaxHP]?.Value ?? 0;
        MaxMPr += user.Character.TemporaryStats[TemporaryStatType.MaxMP]?.Value ?? 0;

        AttackSpeed += user.Character.TemporaryStats[TemporaryStatType.Booster]?.Value ?? 0;

        var sharpEyesStat = user.Character.TemporaryStats[TemporaryStatType.SharpEyes];
        if (sharpEyesStat != null)
        {
            Cr += sharpEyesStat.Value >> 8;
            CDMax += sharpEyesStat.Value & 0xFF;
        }
        
        var thornsEffectStat = user.Character.TemporaryStats[TemporaryStatType.ThornsEffect];
        if (thornsEffectStat != null)
        {
            Cr += thornsEffectStat.Value >> 8;
            CDMax += thornsEffectStat.Value & 0xFF;
        }
            
        var superBodyStat = user.Character.TemporaryStats[TemporaryStatType.SuperBody];
        
        var darkAuraStat = user.Character.TemporaryStats[TemporaryStatType.DarkAura];
        if (darkAuraStat != null)
        {
            PADr += darkAuraStat.Value;
            MADr += darkAuraStat.Value;
        }
        
        var yellowAuraStat = user.Character.TemporaryStats[TemporaryStatType.YellowAura];
        if (yellowAuraStat != null)
        {
            var yellowAuraSkill = await user.StageUser.Context.Templates.Skill.Retrieve(superBodyStat?.Reason ?? yellowAuraStat.Reason);
            var yellowAuraLevel = yellowAuraSkill?[SkillLevels[superBodyStat != null ? Skill.BmageAuraYellow : yellowAuraStat.Reason]];
            
            Speed += yellowAuraLevel?.X ?? 0;
            AttackSpeed += yellowAuraLevel?.Y ?? 0;
        }

        if (JobConstants.GetJobRace(user.Character.Job) == 3 &&
            JobConstants.GetJobType(user.Character.Job) == 3 &&
            user.Character.TemporaryStats.RideVehicleRecord?.Reason == Skill.WildhunterJaguarRiding)
        {
            var jaguarRidingSkill = await user.StageUser.Context.Templates.Skill.Retrieve(Skill.WildhunterJaguarRiding);
            var jaguarRidingLevel = jaguarRidingSkill?[SkillLevels[Skill.WildhunterJaguarRiding]];

            MaxHPr += jaguarRidingLevel?.W ?? 0;
            Speed += jaguarRidingLevel?.X ?? 0;
            EVA += jaguarRidingLevel?.Y ?? 0;
            Cr += jaguarRidingLevel?.Z ?? 0;
        }

        if (user.Character.TemporaryStats.PartyBoosterRecord?.IsActive() ?? false)
            AttackSpeed += user.Character.TemporaryStats.PartyBoosterRecord.Value;

        if (user.Character.TemporaryStats[TemporaryStatType.Frozen] != null)
        {
            if (AttackSpeedBase < 10) 
                AttackSpeedBase += (int)((user.Character.TemporaryStats[TemporaryStatType.Frozen]?.Value ?? 0) * (10 - AttackSpeedBase) / 100d);;
            if (AttackSpeed < 10)
                AttackSpeed += (int)((user.Character.TemporaryStats[TemporaryStatType.Frozen]?.Value ?? 0) * (10 - AttackSpeed) / 100d);;
        }

        if (user.Character.TemporaryStats[TemporaryStatType.Dice] != null)
        {
            MaxHPr += user.Character.TemporaryStats.DiceInfo.MHPr;
            MaxMPr += user.Character.TemporaryStats.DiceInfo.MMPr;
            Cr += user.Character.TemporaryStats.DiceInfo.Cr;
            CDMin += user.Character.TemporaryStats.DiceInfo.CDMin;
            EVAr += user.Character.TemporaryStats.DiceInfo.EVAr;
            Ar += user.Character.TemporaryStats.DiceInfo.Ar;
            Er += user.Character.TemporaryStats.DiceInfo.Er;
            PDDr += user.Character.TemporaryStats.DiceInfo.PDDr;
            MDDr += user.Character.TemporaryStats.DiceInfo.MDDr;
            // PDr += user.Character.TemporaryStats.DiceInfo.PDr;
            // MDr += user.Character.TemporaryStats.DiceInfo.MDr;
            // DIPr += user.Character.TemporaryStats.DiceInfo.DIPr;
            PDamR += user.Character.TemporaryStats.DiceInfo.PDamr;
            MDamR += user.Character.TemporaryStats.DiceInfo.MDamr;
            PADr += user.Character.TemporaryStats.DiceInfo.PADr;
            MADr += user.Character.TemporaryStats.DiceInfo.MADr;
            // EXPr += user.Character.TemporaryStats.DiceInfo.EXPr;
            // IMPr += user.Character.TemporaryStats.DiceInfo.IMPr;
            // ASRr += user.Character.TemporaryStats.DiceInfo.ASRr;
            // TERr += user.Character.TemporaryStats.DiceInfo.TERr;
            // MESOr += user.Character.TemporaryStats.DiceInfo.MESOr;
        }
    }

    private async Task<Tuple<int, int>> GetMastery(IFieldUser user, int skillID)
    {
        var skillTemplate = await user.StageUser.Context.Templates.Skill.Retrieve(skillID);
        var skillLevelTemplate = skillTemplate?[SkillLevels[skillID]];
        return skillLevelTemplate == null 
            ? new Tuple<int, int>(0, 0) 
            : new Tuple<int, int>(skillLevelTemplate.Mastery, skillLevelTemplate.X);
    }
    
    private async Task ApplyMastery(IFieldUser user)
    {
        var weaponType = ItemConstants.GetWeaponType(
            user.Character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) ?? false
                ? result1.ID 
                : 0);
        var subWeaponType = ItemConstants.GetWeaponType(
            user.Character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Shield, out var result2) ?? false
                ? result2.ID 
                : 0);
        var incMastery = 0;
        var incPAD = 0;
        var incMAD = 0;
        var incACC = 0;
        
        if (JobConstants.GetJobType(user.Character.Job) == 2)
        {
            var skills = new List<int>();

            switch (JobConstants.GetJobRace(user.Character.Job))
            {
                case 0:
                    skills.Add(JobConstants.GetJobBranch(user.Character.Job) switch
                    {
                        1 => Skill.Wizard1SpellMastery,
                        2 => Skill.Wizard2SpellMastery,
                        3 => Skill.ClericSpellMastery,
                        _ => 0
                    });
                    break;
                case 1:
                    skills.Add(Skill.FlamewizardSpellMastery);
                    break;
                case 2:
                    skills.Add(Skill.EvanSpellMastery);
                    skills.Add(Skill.EvanMagicMastery);
                    break;
                case 3:
                    skills.Add(Skill.BmageSpellMastery);
                    break;
            }

            foreach (var skill in skills.TakeWhile(_ => incMastery == 0))
                (incMastery, incMAD) = await GetMastery(user, skill);
        }
        
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
                        (incMastery, incACC) = await GetMastery(user, skill);

                        if (skill != Skill.PageWeaponMastery || incMastery <= 0 ||
                            user.Character.TemporaryStats[TemporaryStatType.WeaponCharge] == null) continue;

                        var (incMastery2, _) = await GetMastery(user, Skill.PaladinAdvancedCharge);
                        if (incMastery2 > 0)
                            incMastery = incMastery2;
                    }
                    break;
                }
            case WeaponType.OneHandedAxe:
            case WeaponType.TwoHandedAxe:
                (incMastery, incACC) = await GetMastery(user, Skill.FighterWeaponMastery);
                break;
            case WeaponType.OneHandedMace:
            case WeaponType.TwoHandedMace:
            {
                (incMastery, incACC) = await GetMastery(user, Skill.PageWeaponMastery);
                
                if (incMastery > 0 && user.Character.TemporaryStats[TemporaryStatType.WeaponCharge] != null)
                {
                    var (incMastery2, _) = await GetMastery(user, Skill.PaladinAdvancedCharge);
                    if (incMastery2 > 0)
                        incMastery = incMastery2;
                }
                break;
            }
            case WeaponType.Spear:
            case WeaponType.Polearm:
            {
                if (weaponType == WeaponType.Polearm && 
                    JobConstants.GetJobRace(user.Character.Job) == 2 && 
                    JobConstants.GetJobType(user.Character.Job) == 1)
                {
                    (incMastery, incACC) = await GetMastery(user, Skill.AranPolearmMastery);
                    if (user.Stats.SkillLevels[Skill.AranHighMastery] > 0)
                        (incMastery, incPAD) = await GetMastery(user, Skill.AranHighMastery);
                    break;
                }
                
                (incMastery, incACC) = await GetMastery(user, Skill.SpearmanWeaponMastery);
                
                if (user.Character.TemporaryStats[TemporaryStatType.Beholder] != null)
                    (incMastery, _) = await GetMastery(user, Skill.DarkknightBeholder);
                break;
            }
            case WeaponType.Bow:
                if (JobConstants.GetJobRace(user.Character.Job) == 1)
                {
                    (incMastery, incACC) = await GetMastery(user, Skill.WindbreakerBowMastery);
                    if (user.Stats.SkillLevels[Skill.WindbreakerBowExpert] > 0)
                        (incMastery, incPAD) = await GetMastery(user, Skill.WindbreakerBowExpert);
                    break;
                }
                
                (incMastery, incACC) = await GetMastery(user, Skill.HunterBowMastery);
                if (user.Stats.SkillLevels[Skill.BowmasterBowExpert] > 0)
                    (incMastery, incPAD) = await GetMastery(user, Skill.BowmasterBowExpert);
                break;
            case WeaponType.Crossbow:
                if (JobConstants.GetJobRace(user.Character.Job) == 3)
                {
                    (incMastery, incACC) = await GetMastery(user, Skill.WildhunterCrossbowMastery);
                    if (user.Stats.SkillLevels[Skill.WildhunterCrossbowExpert] > 0)
                        (incMastery, incPAD) = await GetMastery(user, Skill.WildhunterCrossbowExpert);
                    break;
                }
                
                (incMastery, incACC) = await GetMastery(user, Skill.CrossbowmanCrossbowMastery);
                if (user.Stats.SkillLevels[Skill.CrossbowmasterCrossbowExpert] > 0)
                    (incMastery, incPAD) = await GetMastery(user, Skill.CrossbowmasterCrossbowExpert);
                break;
            case WeaponType.ThrowingGlove:
                (incMastery, incACC) = JobConstants.GetJobRace(user.Character.Job) == 1
                    ? await GetMastery(user, Skill.NightwalkerJavelinMastery)
                    : await GetMastery(user, Skill.AssassinJavelinMastery);
                break;
            case WeaponType.Dagger:
                (incMastery, incACC) = subWeaponType == WeaponType.SubDagger
                    ? await GetMastery(user, Skill.Dual1DualMastery)
                    : await GetMastery(user, Skill.ThiefDaggerMastery);
                break;
            case WeaponType.Knuckle:
                (incMastery, incACC) = JobConstants.GetJobRace(user.Character.Job) == 1
                    ? await GetMastery(user, Skill.StrikerKnuckleMastery)
                    : await GetMastery(user, Skill.InfighterKnuckleMastery);
                break;
            case WeaponType.Gun:
                if (JobConstants.GetJobRace(user.Character.Job) == 3)
                {
                    (incMastery, incACC) = await GetMastery(user, Skill.MechanicGunMastery);
                    if (user.Stats.SkillLevels[Skill.MechanicHn07Upgrade] > 0)
                        (incMastery, _) = await GetMastery(user, Skill.MechanicHn07Upgrade);
                    break;
                }
                
                (incMastery, incACC) = await GetMastery(user, Skill.GunslingerGunMastery);
                break;
        }
        
        Mastery += incMastery;
        PAD += incPAD;
        MAD += incMAD;
        ACC += incACC;
    }
    
    private Task ApplyDamage(IFieldUser user)
    {
        var weaponType = ItemConstants.GetWeaponType(
            user.Character.Inventories[ItemInventoryType.Equip]?.Items.TryGetValue(-(short)BodyPart.Weapon, out var result1) ?? false
                ? result1.ID 
                : 0);
        
        var stat1 = 0;
        var stat2 = 0;
        var stat3 = 0;
        var attack = PAD;
        var multiplier = 1.0;
        
        if (JobConstants.GetJobLevel(user.Character.Job) == 0)
        {
            stat1 = STR;
            stat2 = DEX;
            multiplier = 1.2;
        }
        else if (JobConstants.GetJobType(user.Character.Job) == JobType.Magician)
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
        
        var masteryMultiplier = Mastery / 100d;

        masteryMultiplier += ItemConstants.GetMasteryConstByWeaponType(weaponType);
        masteryMultiplier = Math.Min(0.95, masteryMultiplier);

        DamageMax = (int)((stat3 + stat2 + 4 * stat1) / 100d * attack * multiplier + 0.5);
        DamageMin = (int)(masteryMultiplier * DamageMax + 0.5);

        DamageMin = Math.Min(DamageMin, DamageMax);
        DamageMin = Math.Min(Math.Max(DamageMin, 1), 999999);
        DamageMax = Math.Min(Math.Max(DamageMax, 1), 999999);
        return Task.CompletedTask;
    }
}
