namespace Edelstein.Common.Gameplay.Constants;

public static class ItemConstants
{
    public static WeaponType GetWeaponType(int templateID)
        => (WeaponType)(templateID / 10000 % 100);
}
