using Edelstein.Core.Types;

namespace Edelstein.Core.Gameplay.Constants
{
    public class SkillConstants
    {
        public static bool IsExtendSPJob(int job)
        {
            return job / 1000 == 3 || job / 100 == 22 || job == 2001;
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
    }
}