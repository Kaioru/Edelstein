using System;

namespace Edelstein.Core.Gameplay.Constants
{
    public class ItemConstants
    {
        public static DateTime Permanent = DateTime.FromFileTimeUtc(150842304000000000);

        public static bool IsRechargeableItem(int templateID)
        {
            var type = templateID / 10000;
            return type == 207 || type == 233;
        }
    }
}