using System;
using Edelstein.Core.Types;

namespace Edelstein.Core.Gameplay.Constants
{
    public class GameConstants
    {
        public static int GetStartField(Race race)
        {
            switch (race)
            {
                case Race.Resistance:
                    return 931000000;
                default:
                case Race.Normal:
                    return 0;
                case Race.Cygnus:
                    return 130030000;
                case Race.Aran:
                    return 914000000;
                case Race.Evan:
                    return 900090000;
            }
        }

        public static short GetStartJob(Race race)
        {
            switch (race)
            {
                case Race.Resistance:
                    return (short) Job.Citizen;
                default:
                case Race.Normal:
                    return (short) Job.Novice;
                case Race.Cygnus:
                    return (short) Job.Noblesse;
                case Race.Aran:
                    return (short) Job.Legend;
                case Race.Evan:
                    return (short) Job.Evan;
            }
        }
    }
}