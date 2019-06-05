using Edelstein.Core.Types;

namespace Edelstein.Core.Gameplay.Constants
{
    public class GameConstants
    {
        public static readonly int[] CharacterEXPTable;
        public static readonly int[] PetTamenessTable;
        public static readonly int[] TamingMobTamenessTable;

        static GameConstants()
        {
            CharacterEXPTable = new int[201];
            CharacterEXPTable[0] = 15;
            CharacterEXPTable[1] = 15;
            CharacterEXPTable[2] = 34;
            CharacterEXPTable[3] = 57;
            CharacterEXPTable[4] = 92;
            CharacterEXPTable[5] = 135;
            CharacterEXPTable[6] = 372;
            CharacterEXPTable[7] = 560;
            CharacterEXPTable[8] = 840;
            CharacterEXPTable[9] = 1242;

            for (var i = 10; i < 15; i++) CharacterEXPTable[i] = CharacterEXPTable[i - 1];

            for (var i = 15; i < 30; i++)
                CharacterEXPTable[i] = (int) (CharacterEXPTable[i - 1] * 1.2 + 0.5);

            for (var i = 30; i < 35; i++) CharacterEXPTable[i] = CharacterEXPTable[i - 1];

            for (var i = 35; i < 40; i++)
                CharacterEXPTable[i] = (int) (CharacterEXPTable[i - 1] * 1.2 + 0.5);
            for (var i = 40; i < 70; i++)
                CharacterEXPTable[i] = (int) (CharacterEXPTable[i - 1] * 1.08 + 0.5);

            for (var i = 70; i < 75; i++) CharacterEXPTable[i] = CharacterEXPTable[i - 1];

            for (var i = 75; i < 120; i++)
                CharacterEXPTable[i] = (int) (CharacterEXPTable[i - 1] * 1.07 + 0.5);

            for (var i = 120; i < 125; i++) CharacterEXPTable[i] = CharacterEXPTable[i - 1];

            for (var i = 125; i < 160; i++)
                CharacterEXPTable[i] = (int) (CharacterEXPTable[i - 1] * 1.07 + 0.5);
            for (var i = 160; i < 200; i++)
                CharacterEXPTable[i] = (int) (CharacterEXPTable[i - 1] * 1.06 + 0.5);

            CharacterEXPTable[200] = 0;

            PetTamenessTable = new[]
            {
                0x1, 0x3, 0x6, 0xE, 0x1F, 0x3C, 0x6C, 0x0B5, 0x11F, 0x1B2,
                0x278, 0x37B, 0x4C8, 0x66A, 0x871, 0x0AE9, 0x0DE5, 0x1173, 0x15A6, 0x1A91,
                0x2047, 0x26DE, 0x2E6A, 0x3704, 0x40C2, 0x4BBF, 0x5813, 0x65DA, 0x7530, 0x0
            };
            TamingMobTamenessTable = new[]
            {
                0x0, 0x14, 0x2D, 0x4B, 0x6F, 0x99, 0x0C9, 0x100, 0x13F, 0x185,
                0x1D3, 0x22A, 0x28B, 0x2F5, 0x36A, 0x3EB, 0x478, 0x511, 0x5B8, 0x66E,
                0x734, 0x80A, 0x8F2, 0x9ED, 0x0AFC, 0x0C21, 0x0D5C, 0x0EB0, 0x101E, 0x11A8
            };
        }

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