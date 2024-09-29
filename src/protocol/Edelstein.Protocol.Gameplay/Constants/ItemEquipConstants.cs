namespace Edelstein.Protocol.Gameplay.Constants;

public static class ItemEquipConstants
{
    public static WeaponType GetWeaponType(this int itemID)
        => itemID == 0 ? WeaponType.Barehand : (WeaponType)(itemID / 10000 % 100);

    public static double GetMasteryConst(this WeaponType type)
        => type switch
        {
            WeaponType.Wand or
                WeaponType.Staff => 0.25,
            WeaponType.Bow or
                WeaponType.Crossbow or
                WeaponType.ThrowingGlove or
                WeaponType.Gun => 0.15,
            _ => 0.20,
        };
}
