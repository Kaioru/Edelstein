namespace Edelstein.Core.Constants
{
    public static class ItemConstants
    {
        public static bool IsRechargeableItem(int templateID)
        {
            var type = templateID / 10000;
            return type == 207 || type == 233;
        }
    }
}