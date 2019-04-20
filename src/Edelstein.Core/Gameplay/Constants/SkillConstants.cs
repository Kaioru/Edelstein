namespace Edelstein.Core.Gameplay.Constants
{
    public class SkillConstants
    {
        public static bool IsExtendSPJob(int job)
        {
            return job / 1000 == 3 || job / 100 == 22 || job == 2001;
        }
    }
}