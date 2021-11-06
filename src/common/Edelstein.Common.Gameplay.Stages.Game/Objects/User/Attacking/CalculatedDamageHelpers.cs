using System;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    // TODO remove this class, reorganise
    public static class CalculatedDamageHelpers
    {
        public static double GetRandom(uint rand, double f0, double f1)
        {
            if (f1 != f0)
            {
                if (f0 > f1)
                {
                    var tmp = f1;
                    f0 = f1;
                    f1 = tmp;
                }

                return f0 + rand % 10000000 * (f1 - f0) / 9999999.0;
            }
            else return f0;
        }
    }
}
