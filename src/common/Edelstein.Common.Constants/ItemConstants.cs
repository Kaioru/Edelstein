namespace Edelstein.Common.Gameplay.Constants;

public static class ItemConstants
{
    public static bool IsStatChangeItem(this int itemID)
        => itemID / 10000 is 
            200 or 
            201 or
            202 or
            205 or 
            221 or
            236 or
            238 or
            245;
}
