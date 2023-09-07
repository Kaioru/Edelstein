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

    public static Element GetElementByChargedSkill(int skillID) 
        => skillID switch
        {
            Skill.KnightFireCharge => Element.Fire,
            Skill.KnightIceCharge => Element.Ice,
            Skill.KnightLightningCharge => Element.Light,
            Skill.PaladinDivineCharge => Element.Holy,
            _ => Element.Physical
        };
}
