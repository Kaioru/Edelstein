using System;
using Edelstein.Common.Gameplay.Constants.Types;

namespace Edelstein.Common.Gameplay.Constants
{
    public static class GameConstants
    {
        public static DateTime Permanent = DateTime.FromFileTimeUtc(150842304000000000);

        public static bool IsExtendSPJob(int job)
            => job / 1000 == 3 || job / 100 == 22 || job == 2001;

        public static int GetJobLevel(int job)
        {
            if (job % 100 > 0 && job != 2001)
                return (job / 10 == 43
                           ? (job - 430) / 2
                           : job % 10
                       ) + 2;
            return job % 1000 == 0 || job == 2001 ? 0 : 1;
        }

        public static bool IsEvanJob(int job)
        {
            switch ((Job)job)
            {
                case Job.EvanJr:
                case Job.Evan:
                case Job.Evan2:
                case Job.Evan3:
                case Job.Evan4:
                case Job.Evan5:
                case Job.Evan6:
                case Job.Evan7:
                case Job.Evan8:
                case Job.Evan9:
                case Job.Evan10:
                    return true;
            }

            return false;
        }

        public static bool IsIgnoreMasterLevelForCommon(int skill)
        {
            switch ((Skill)skill)
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
                switch ((Skill)skill)
                {
                    case Skill.EvanMagicGuard:
                    case Skill.EvanMagicBooster:
                    case Skill.EvanMagicCritical:
                        return true;
                }

                return GetJobLevel(job) == 9 || GetJobLevel(job) == 10;
            }

            switch ((Skill)skill)
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
