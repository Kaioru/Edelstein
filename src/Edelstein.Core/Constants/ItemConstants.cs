using System;

namespace Edelstein.Core.Constants
{
    public static class ItemConstants
    {
        public static DateTime Permanent = DateTime.FromFileTimeUtc(150842304000000000);

        public static bool IsRechargeableItem(int templateID)
        {
            var type = templateID / 10000;
            return type == 207 || type == 233;
        }

        public static bool IsTreatSingly(int templateID)
        {
            var type = templateID / 1000000;

            if (type == 2 || type == 3 || type == 4)
            {
                var subType = templateID / 10000;
                if (subType != 207 && subType != 233)
                    return false;
            }

            return true;
        }
    }
}