namespace Edelstein.Core.Types
{
    public enum Race
    {
        Resistance = 0x0,
        Normal = 0x1,
        Cygnus = 0x2,
        Aran = 0x3,
        Evan = 0x4
    }

    public class RaceConstants
    {
        public static int ChooseStartingMap(Race race)
        {
            switch (race)
            {
                case Race.Resistance:
                    return 931000000; // Dangerous Hide-and-Seek : Neglected Rocky Mountain - Resi_tutor10.Lua
                case Race.Normal:
                    return 10000; // Maple Road : Mushroom Park
                case Race.Cygnus:
                    return 130030000; // Empress's Road : Forest of Beginning 1
                case Race.Aran:
                    return 914000000; // Black Road : Wounded Soldier's Camp - aranTutorAlone.Lua missing
                case Race.Evan:
                    return 900090000; // Video : Teaser - scene0
                default:
                    throw new ArgumentOutOfRangeException(nameof(race), race, null);
            }
        }

        public static short ChooseStartingJob(Race race)
        {
            switch (race)
            {
                case Race.Resistance:
                    return (short)Job.Citizen;
                case Race.Normal:
                    return (short)Job.Novice; // Beginner
                case Race.Cygnus:
                    return (short)Job.Noblesse;
                case Race.Aran:
                    return (short)Job.Legend;
                case Race.Evan:
                    return (short)Job.Evan;
                default:
                    throw new ArgumentOutOfRangeException(nameof(race), race, null);
            }
        }
    }
}