using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;

namespace Edelstein.Common.Gameplay.Constants;

public static class SkillConstants
{
    public static bool IsIgnoreMasterLevelForCommon(int skill)
    {
        switch (skill)
        {
            case Skill.HeroCombatMastery:
            case Skill.PaladinBlessingArmor:
            case Skill.DarkknightBeholdersRevenge:
            case Skill.Archmage1MasterMagic:
            case Skill.Archmage2MasterMagic:
            case Skill.BishopMasterMagic:
            case Skill.BowmasterMarkmanShip:
            case Skill.CrossbowmasterMarkmanShip:
            case Skill.NightlordExpertJavelin:
            case Skill.ShadowerGrid:
            case Skill.ViperCounterAttack:
            case Skill.CaptainCounterAttack:
            case Skill.BmageEnergize:
            case Skill.WildhunterWildInstinct:
                return true;
            default:
                return false;
        }
    }
    
    public static bool IsSkillNeedMasterLevel(int skill)
    {
        if (IsIgnoreMasterLevelForCommon(skill)) return false;
        var job = skill / 10000;

        if (
            job == Job.EvanJr || 
            JobConstants.GetJobRace(job) == JobRace.Third && JobConstants.GetJobType(job) == JobType.Magician
        )
        {
            switch (skill)
            {
                case Skill.EvanMagicGuard:
                case Skill.EvanMagicBooster:
                case Skill.EvanMagicCritical:
                    return true;
            }

            return JobConstants.GetJobLevel(job) == 9 || JobConstants.GetJobLevel(job) == 10;
        }

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
    {
        switch (skill)
        {
            case Skill.Archmage1Bigbang:
            case Skill.Archmage2Bigbang:
            case Skill.BishopBigbang:
            case Skill.BowmasterStormArrow:
            case Skill.CrossbowmasterPiercing:
            case Skill.Dual5FinalCut:
            case Skill.Dual5MonsterBomb:
            case Skill.InfighterScrewPunch:
            case Skill.GunslingerThrowingBomb:
            case Skill.CaptainRapidFire:
            case Skill.WindbreakerStormArrow:
            case Skill.NightwalkerPoisonBomb:
            case Skill.StrikerScrewPunch:
            case Skill.EvanIceBreath:
            case Skill.EvanBreath:
            case Skill.WildhunterSwallow:
            case Skill.WildhunterWildShoot:
            case Skill.MechanicFlamethrower:
            case Skill.MechanicFlamethrowerUp:
                return true;
        }
        return false;
    }

    public static int GetMaxGaugeTime(int skillID)
    {
        if (!IsKeydownSkill(skillID)) return 0;

        switch (skillID)
        {
            case Skill.Archmage1Bigbang:
            case Skill.Archmage2Bigbang:
            case Skill.BishopBigbang:
                return 1000;
        }
        
        return 500;
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
        => action  is 41 or 57;
    
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
