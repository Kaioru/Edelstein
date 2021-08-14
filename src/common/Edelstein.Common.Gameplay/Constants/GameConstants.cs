using Edelstein.Common.Gameplay.Constants.Types;

namespace Edelstein.Common.Gameplay.Constants
{
    public static class GameConstants
    {
        public static bool IsExtendSPJob(int job)
            => job / 1000 == 3 || job / 100 == 22 || job == 2001;

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
    }
}
