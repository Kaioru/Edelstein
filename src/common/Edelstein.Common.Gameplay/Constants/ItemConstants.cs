namespace Edelstein.Common.Gameplay.Constants;

public static class ItemConstants
{
    public static WeaponType GetWeaponType(int templateID)
        => (WeaponType)(templateID / 10000 % 100);

    public static double GetMasteryConstByWeaponType(WeaponType weaponType)
        => weaponType switch
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
