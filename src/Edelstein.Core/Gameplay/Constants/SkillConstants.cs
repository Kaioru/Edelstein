using Edelstein.Core.Types;

namespace Edelstein.Core.Gameplay.Constants
{
    public class SkillConstants
    {
        public static bool IsExtendSPJob(int job)
        {
            return job / 1000 == 3 || job / 100 == 22 || job == 2001;
        }

        public static int GetJobLevel(int job)
        {
            if (job % 100 > 0 && job != 2001)
                return (job / 10 == 43
                           ? (job - 430) / 2
                           : job % 10
                       ) + 2;
            return 1;
        }

        public static bool IsEvanJob(int job)
        {
            return job / 100 == 22 || job == 2001;
        }

        public static bool IsKeydownSkill(int skill)
        {
            switch ((Skill) skill)
            {
                case Skill.Archmage1Bigbang:
                case Skill.Archmage2Bigbang:
                case Skill.BishopBigbang:
                case Skill.BowmasterStormArrow:
                case Skill.CrossbowmasterPiercing:
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
                default:
                    return false;
            }
        }

        public static bool IsIgnoreMasterLevelForCommon(int skill)
        {
            switch ((Skill) skill)
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

            if (IsEvanJob(job))
            {
                switch ((Skill) skill)
                {
                    case Skill.EvanMagicGuard:
                    case Skill.EvanMagicBooster:
                    case Skill.EvanMagicCritical:
                        return true;
                }

                return GetJobLevel(job) == 9 || GetJobLevel(job) == 10;
            }

            switch ((Skill) skill)
            {
                case Skill.Dual2SlashStorm:
                case Skill.Dual3HustleDash:
                case Skill.Dual4MirrorImaging:
                case Skill.Dual4FlyingAssaulter:
                    return true;
            }

            if (job == 100 * (job / 100)) return false;
            return GetJobLevel(job) == 4;
        }
    }
}