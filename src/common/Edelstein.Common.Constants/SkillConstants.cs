using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Constants;

public static class SkillConstants
{
    public static bool IsIgnoreMasterLevelForCommon(int skill)
        => skill switch
        {
            Skill.HeroCombatMastery => true,
            Skill.PaladinBlessingArmor => true,
            Skill.DarkknightBeholdersRevenge => true,
            Skill.Archmage1MasterMagic => true,
            Skill.Archmage2MasterMagic => true,
            Skill.BishopMasterMagic => true,
            Skill.BowmasterMarkmanShip => true,
            Skill.CrossbowmasterMarkmanShip => true,
            Skill.CrossbowmasterUltimateStrafe => true,
            Skill.NightlordExpertJavelin => true,
            Skill.ShadowerGrid => true,
            Skill.ViperCounterAttack => true,
            Skill.CaptainCounterAttack => true,
            Skill.BmageEnergize => true,
            Skill.WildhunterWildInstinct => true,
            _ => false
        };

    public static bool IsSkillNeedMasterLevel(int skill)
    {
        if (IsIgnoreMasterLevelForCommon(skill)) return false;

        var job = skill / 10000;

        if (JobConstants.IsEvanJob(job))
            return skill switch
            {
                Skill.EvanMagicGuard => true,
                Skill.EvanMagicBooster => true,
                Skill.EvanMagicCritical => true,
                _ => JobConstants.GetJobLevel(job) == 9 || JobConstants.GetJobLevel(job) == 10
            };

        switch (skill)
        {
            case Skill.Dual2SlashStorm:
            case Skill.Dual3HustleDash:
            case Skill.Dual4MirrorImaging:
            case Skill.Dual4FlyingAssaulter:
                return true;
        }

        if (job == 100 * (job / 100)) return false;

        return JobConstants.GetJobLevel(job) == 4;
    }

    public static bool IsKeydownSkill(int skill)
        => skill switch
        {
            Skill.Archmage1Bigbang => true,
            Skill.Archmage2Bigbang => true,
            Skill.BishopBigbang => true,
            Skill.BowmasterStormArrow => true,
            Skill.CrossbowmasterPiercing => true,
            Skill.Dual5FinalCut => true,
            Skill.Dual5MonsterBomb => true,
            Skill.InfighterScrewPunch => true,
            Skill.GunslingerThrowingBomb => true,
            Skill.CaptainRapidFire => true,
            Skill.WindbreakerStormArrow => true,
            Skill.NightwalkerPoisonBomb => true,
            Skill.StrikerScrewPunch => true,
            Skill.EvanIceBreath => true,
            Skill.EvanBreath => true,
            Skill.WildhunterSwallow => true,
            Skill.WildhunterWildShoot => true,
            Skill.MechanicFlamethrower => true,
            Skill.MechanicFlamethrowerUp => true,
            _ => false
        };

    public static int GetMaxGaugeTime(int skillID)
    {
        if (!IsKeydownSkill(skillID)) return 0;

        return skillID switch
        {
            Skill.Archmage1Bigbang => 1000,
            Skill.Archmage2Bigbang => 1000,
            Skill.BishopBigbang => 1000,
            _ => 500
        };
    }

    public static Element GetElementByChargedSkill(int skillID)
        => skillID switch
        {
            Skill.KnightFireCharge => Element.Fire,
            Skill.KnightIceCharge => Element.Ice,
            Skill.KnightLightningCharge => Element.Light,
            Skill.PaladinDivineCharge => Element.Holy,
            _ => Element.Physical
        };

    public static bool IsCorrectJobForSkillRoot(int jobID, int skillRoot)
    {
        if (skillRoot % 100 == 0)
            return skillRoot / 100 == jobID / 100;

        return skillRoot / 10 == jobID / 10 && jobID % 10 >= skillRoot % 10;
    }

    public static int GetMagicAmplificationSkill(int jobID)
    {
        if (IsCorrectJobForSkillRoot(jobID, Job.MageFirePoison))
            return Skill.Mage1ElementAmplification;
        if (IsCorrectJobForSkillRoot(jobID, Job.MageThunderCold))
            return Skill.Mage2ElementAmplification;
        if (IsCorrectJobForSkillRoot(jobID, Job.Flamewizard3))
            return Skill.FlamewizardElementAmplification;
        if (IsCorrectJobForSkillRoot(jobID, Job.Evan3))
            return Skill.EvanElementAmplification;

        return 0;
    }

    public static bool IsShootAction(int action)
        => action is >= 31 and <= 36 or 75 or 116 or 111 or 100 or 109 or 110 or 122 or 115 or 123 or 142 or 268 or 269 or 200 or 203;

    public static bool IsProneStabAction(int action)
        => action is 41 or 57;

    public static bool IsJaguarMeleeAttackSkill(int skillID)
        => skillID switch
        {
            Skill.WildhunterJaguarNuckback => true,
            Skill.WildhunterSwallowDummyAttack => true,
            Skill.WildhunterCrossRoad => true,
            Skill.WildhunterClawCut => true,
            Skill.WildhunterElrectronicshock => true,
            _ => false
        };
}
