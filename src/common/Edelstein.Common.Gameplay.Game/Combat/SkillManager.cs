using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Common.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Game.Objects.Summoned;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class SkillManager : ISkillManager
{
    private readonly ITemplateManager<ISkillTemplate> _skills;

    public SkillManager(ITemplateManager<ISkillTemplate> skills) 
        => _skills = skills;

    public async Task ProcessUserAttackMob(IFieldUser user, IFieldMob mob, int skillID, int damage)
    {
        await mob.Damage(damage, user);

        if (mob.HP <= 0) return;

        var weaponChargeStat = user.Character.TemporaryStats[TemporaryStatType.WeaponCharge];
        if (weaponChargeStat?.Reason == Skill.KnightIceCharge)
        {
            var weaponChargeSkill = await _skills.Retrieve(weaponChargeStat.Reason);
            var weaponChargeLevel = weaponChargeSkill?[user.Stats.SkillLevels[weaponChargeStat.Reason]];

            await mob.ModifyTemporaryStats(m => m.Set(
                MobTemporaryStatType.Freeze,
                1,
                weaponChargeStat.Reason,
                DateTime.UtcNow.AddSeconds(weaponChargeLevel?.Y ?? 0)));
        }

        var random = new Random();
        var skill = await _skills.Retrieve(skillID);
        var level = skill?[user.Stats.SkillLevels[skillID]];
        
        if (skill == null || level == null) return;
        if (level.Prop > 0 && random.Next(0, 100) > level.Prop) return;
        if (level.SubProp > 0 && random.Next(0, 100) > level.SubProp) return;

        var now = DateTime.UtcNow;
        var mobStats = new List<Tuple<MobTemporaryStatType, short>>();
        var mobBurnedInfo = new List<IMobBurnedInfo>();
        var expire = now.AddSeconds(level.Time);
        
        if (level.Dot > 0)
            mobBurnedInfo.Add(new MobBurnedInfo(
                user.Character.ID,
                skillID,
                await user.Damage.CalculateBurnedDamage(
                    user.Character,
                    user.Stats,
                    mob,
                    mob.Stats,
                    skillID,
                    user.Stats.SkillLevels[skillID]
                ),
                TimeSpan.FromSeconds(level.DotInterval),
                now,
                now.AddSeconds(level.DotTime)
            ));
        
        switch (skillID)
        {
            case Skill.CrusaderPanic:
            case Skill.SoulmasterPanicSword:
                // TODO: not working?
                mobStats.Add(Tuple.Create(MobTemporaryStatType.Darkness, level.X));
                break;
            case Skill.CrusaderComa:
            case Skill.SoulmasterComaSword:
            case Skill.CrusaderShout:
            case Skill.KnightChargeBlow:
            case Skill.HeroMonsterMagnet:
            case Skill.DarkknightMonsterMagnet:
            case Skill.Mage1TeleportMastery:
            case Skill.Mage2TeleportMastery:
            case Skill.PriestTeleportMastery:
            case Skill.BmageTeleportMastery:
                mobStats.Add(Tuple.Create(MobTemporaryStatType.Stun, (short)1));
                break;
                
        }
        
        if (mobStats.Count > 0 || mobBurnedInfo.Count > 0)
            await mob.ModifyTemporaryStats(s =>
            {
                if (mobStats.Count > 0)
                {
                    s.ResetByReason(skillID);
                    foreach (var tuple in mobStats)
                        s.Set(tuple.Item1, tuple.Item2, skillID, expire);
                }

                if (mobBurnedInfo.Count > 0)
                {
                    foreach (var burned in mobBurnedInfo)
                        s.SetBurnedInfo(burned);
                }
            });
    }
    
    public async Task<bool> ProcessUserAttack(IFieldUser user, int skillID, bool isHitAnyMob)
    {
        var skill = await _skills.Retrieve(skillID);
        var level = skill?.Levels[user.Stats.SkillLevels[skillID]];

        if (skill == null || level == null) return skillID == 0;
        if (level.MPCon > user.Character.MP) return false;
        
        var random = new Random();
        
        var comboCounterStat = user.Character.TemporaryStats[TemporaryStatType.ComboCounter];

        if (comboCounterStat != null && skillID is
            Skill.CrusaderPanic or
            Skill.CrusaderComa or
            Skill.SoulmasterPanicSword or
            Skill.SoulmasterComaSword)
        {
            await user.ModifyTemporaryStats(s => s.Set(
                TemporaryStatType.ComboCounter,
                1,
                comboCounterStat.Reason,
                comboCounterStat.DateExpire
            ));
        }
        else if (comboCounterStat != null && isHitAnyMob)
        {
            var comboCounterSkillID = JobConstants.GetJobRace(user.Character.Job) == 0
                ? Skill.CrusaderComboAttack
                : Skill.SoulmasterComboAttack;
            var comboCounterSkill = await _skills.Retrieve(comboCounterSkillID);
            var comboCounterLevel = comboCounterSkill?[user.Stats.SkillLevels[comboCounterStat.Reason]];
            var comboCounter = comboCounterStat.Value - 1;
            var comboMax = comboCounterLevel?.X ?? 0;

            var advComboCounterSkillID = JobConstants.GetJobRace(user.Character.Job) == 0
                ? Skill.HeroAdvancedCombo
                : Skill.SoulmasterAdvancedCombo;
            var advComboCounterSkill = await _skills.Retrieve(advComboCounterSkillID);
            var advComboCounterLevel = advComboCounterSkill?[user.Stats.SkillLevels[advComboCounterSkillID]];
            var comboDoubleChance = advComboCounterLevel?.Prop ?? 0;
            
            comboMax = advComboCounterLevel?.X ?? comboMax;
            
            if (comboCounter < comboMax)
                await user.ModifyTemporaryStats(s => s.Set(
                    TemporaryStatType.ComboCounter,
                    Math.Min(comboMax + 1, comboCounterStat.Value + (random.Next(0, 100) <= comboDoubleChance ? 2 : 1)),
                    comboCounterStat.Reason,
                    comboCounterStat.DateExpire
                ));
        }

        await user.ModifyStats(s =>
        {
            if (level.MPCon > 0)
                s.MP -= level.MPCon;
        });
        return true;
    }
    
    public async Task<bool> ProcessUserSkill(IFieldUser user, int skillID)
    {
        var skill = await _skills.Retrieve(skillID);
        var skillLevel = user.Stats.SkillLevels[skillID];
        var level = skill?.Levels[skillLevel];

        if (skill == null || level == null) return false;
        if (level.MPCon > user.Character.MP) return false;

        var summoned = new List<FieldSummoned>();
        var stats = new List<Tuple<TemporaryStatType, short>>();
        DateTime? expire = DateTime.UtcNow.AddSeconds(level.Time);

        if (level.PAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.PAD, level.PAD));
        if (level.PDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.PDD, level.PDD));
        if (level.MAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.MAD, level.MAD));
        if (level.MDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.MDD, level.MDD));
        
        if (level.EPAD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EPAD, level.EPAD));
        if (level.EPDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EPDD, level.EPDD));
        if (level.EMDD > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EMDD, level.EMDD));
        
        if (level.ACC > 0)
            stats.Add(Tuple.Create(TemporaryStatType.ACC, level.ACC));
        if (level.EVA > 0)
            stats.Add(Tuple.Create(TemporaryStatType.EVA, level.EVA));

        switch (skill.ID)
        {
            case Skill.FighterWeaponBooster:
            case Skill.PageWeaponBooster:
            case Skill.SpearmanWeaponBooster:
            case Skill.Mage1MagicBooster:
            case Skill.Mage2MagicBooster:
                stats.Add(Tuple.Create(TemporaryStatType.Booster, level.X));
                break;
            case Skill.FighterPowerGuard:
            case Skill.PagePowerGuard:
                stats.Add(Tuple.Create(TemporaryStatType.PowerGuard, level.X));
                break;
            case Skill.SpearmanHyperBody:
                stats.Add(Tuple.Create(TemporaryStatType.MaxHP, level.X));
                stats.Add(Tuple.Create(TemporaryStatType.MaxMP, level.Y));
                break;
            case Skill.CrusaderComboAttack:
            case Skill.SoulmasterComboAttack:
                stats.Add(Tuple.Create(TemporaryStatType.ComboCounter, (short)1));
                break;
            case Skill.KnightFireCharge:
            case Skill.KnightIceCharge:
            case Skill.KnightLightningCharge:
            case Skill.PaladinDivineCharge:
                stats.Add(Tuple.Create(TemporaryStatType.WeaponCharge, level.X));
                break;
            case Skill.KnightCombatOrders:
                stats.Add(Tuple.Create(TemporaryStatType.CombatOrders, level.X));
                break;
            case Skill.Mage1TeleportMastery:
            case Skill.Mage2TeleportMastery:
            case Skill.PriestTeleportMastery:
            case Skill.BmageTeleportMastery:
                expire = null;
                stats.Add(Tuple.Create(TemporaryStatType.TeleportMasteryOn, level.X));
                break;
            case Skill.Mage1ElementalReset:
            case Skill.Mage2ElementalReset:
            case Skill.FlamewizardElementalReset:
            case Skill.EvanElementalReset:
                stats.Add(Tuple.Create(TemporaryStatType.ElementalReset, level.X));
                break;
            case Skill.HeroMapleHero:
            case Skill.PaladinMapleHero:
            case Skill.DarkknightMapleHero:
            case Skill.Archmage1MapleHero:
            case Skill.Archmage2MapleHero:
            case Skill.BishopMapleHero:
            case Skill.BowmasterMapleHero:
            case Skill.CrossbowmasterMapleHero:
            case Skill.NightlordMapleHero:
            case Skill.ShadowerMapleHero:
            case Skill.Dual5MapleHero:
            case Skill.ViperMapleHero:
            case Skill.CaptainMapleHero:
            case Skill.AranMapleHero:
            case Skill.EvanMapleHero:
            case Skill.BmageMapleHero:
            case Skill.WildhunterMapleHero:
            case Skill.MechanicMapleHero:
                stats.Add(Tuple.Create(TemporaryStatType.BasicStatUp, level.X));
                break;
            case Skill.HeroStance:
            case Skill.PaladinStance:
            case Skill.DarkknightStance:
            case Skill.AranFreezeStanding:
            case Skill.BmageStance:
                stats.Add(Tuple.Create(TemporaryStatType.Stance, level.Prop));
                break;
            case Skill.HeroEnrage:
                stats.Add(Tuple.Create(TemporaryStatType.Enrage, (short)(
                    level.X * 100 +
                    level.MobCount
                )));
                break;
            case Skill.DarkknightBeholder:
                stats.Add(Tuple.Create(TemporaryStatType.Beholder, level.X));
                summoned.Add(new FieldSummoned(
                    user,
                    skillID,
                    (byte)skillLevel,
                    MoveAbilityType.Walk,
                    SummonedAssistType.Heal,
                    user.Position,
                    user.Foothold,
                    expire
                ));
                break;
            case Skill.Archmage1Ifrit:
            case Skill.Archmage2Elquines:
            case Skill.FlamewizardIfrit:
                summoned.Add(new FieldSummoned(
                    user,
                    skillID,
                    (byte)skillLevel,
                    MoveAbilityType.Walk,
                    SummonedAssistType.Attack,
                    user.Position,
                    user.Foothold,
                    expire
                ));
                break;
        }

        await user.Modify(m =>
        {
            m.Stats(s => {
                if (level.MPCon > 0)
                    s.MP -= level.MPCon;
            });
            if (stats.Count > 0)
                m.TemporaryStats(s =>
                {
                    if (skill.ID == Skill.HeroEnrage)
                    {
                        var comboCounterStat = user.Character.TemporaryStats[TemporaryStatType.ComboCounter];
                        if (comboCounterStat != null)
                            s.Set(
                                TemporaryStatType.ComboCounter,
                                1,
                                comboCounterStat.Reason,
                                comboCounterStat.DateExpire
                            );
                    }
                    
                    s.ResetByReason(skillID);
                    foreach (var tuple in stats)
                        s.Set(tuple.Item1, tuple.Item2, skillID, expire);
                });
        });

        if (summoned.Count > 0)
        {
            var existingSummons = user.Owned
                .OfType<IFieldSummoned>()
                .ToImmutableList();
            foreach (var summon in summoned)
            {
                var existing = existingSummons
                    .FirstOrDefault(o => o.SkillID == summon.SkillID);

                if (existing != null)
                {
                    user.Owned.Remove(existing);
                    if (user.Field != null)
                        await user.Field.Leave(existing);
                }

                user.Owned.Add(summon);
                if (user.Field != null)
                    await user.Field.Enter(summon, () => summon.GetEnterFieldPacket(1));
            }
        }

        return true;
    }
}
