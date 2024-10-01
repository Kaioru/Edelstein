using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Entities.Inventories;

namespace Edelstein.Protocol.Gameplay.Constants;

public static class ItemEquipConstants
{
    public static bool IsWeapon(this int itemID)
        => (itemID / 100000) switch
        {
            13 or 14 or 16 or 17 => true,
            _ => false
        };

    public static bool IsArmor(this BodyPart bodyPart)
        => bodyPart is 
            BodyPart.Cap or
            BodyPart.Clothes or 
            BodyPart.Pants or 
            BodyPart.Shoes or 
            BodyPart.Gloves or 
            BodyPart.Cape;

    public static bool IsAccessory(this BodyPart bodyPart)
        => bodyPart is 
            BodyPart.FaceAcc or 
            BodyPart.EyeAcc or 
            BodyPart.EarAcc or 
            BodyPart.Ring1 or 
            BodyPart.Ring2 or 
            BodyPart.Ring3 or 
            BodyPart.Ring4 or
            BodyPart.Pendant or
            BodyPart.ExtPendant1 or 
            BodyPart.Belt;

    public static IReadOnlySet<BodyPart> GetBodyParts(this int itemID)
        => (itemID / 10000) switch
        {
            100 => ImmutableHashSet.Create(BodyPart.Cap),
            101 => ImmutableHashSet.Create(BodyPart.FaceAcc),
            102 => ImmutableHashSet.Create(BodyPart.EyeAcc),
            103 => ImmutableHashSet.Create(BodyPart.EarAcc),
            104 or 105 => ImmutableHashSet.Create(BodyPart.Clothes),
            106 => ImmutableHashSet.Create(BodyPart.Pants),
            107 => ImmutableHashSet.Create(BodyPart.Shoes),
            108 => ImmutableHashSet.Create(BodyPart.Gloves),
            109 or 119 or 134 => ImmutableHashSet.Create(BodyPart.Shield),
            110 => ImmutableHashSet.Create(BodyPart.Cape),
            111 => ImmutableHashSet.Create(BodyPart.Ring1, BodyPart.Ring2, BodyPart.Ring3, BodyPart.Ring4),
            112 => ImmutableHashSet.Create(BodyPart.Pendant, BodyPart.ExtPendant1),
            113 => ImmutableHashSet.Create(BodyPart.Belt),
            114 => ImmutableHashSet.Create(BodyPart.Medal),
            115 => ImmutableHashSet.Create(BodyPart.Shoulder),
            161 => ImmutableHashSet.Create(BodyPart.MechanicEngine),
            162 => ImmutableHashSet.Create(BodyPart.MechanicArm),
            163 => ImmutableHashSet.Create(BodyPart.MechanicLeg),
            164 => ImmutableHashSet.Create(BodyPart.MechanicFrame),
            165 => ImmutableHashSet.Create(BodyPart.MechanicTransistor),
            180 => itemID == 1802100 
                ? ImmutableHashSet.Create(BodyPart.PetRingLabel, BodyPart.PetRingLabel2, BodyPart.PetRingLabel3)
                : ImmutableHashSet.Create(BodyPart.PetWear, BodyPart.PetWear2, BodyPart.PetWear3),
            181 => itemID switch
            {
                1812000 => ImmutableHashSet.Create(BodyPart.PetAbilMeso, BodyPart.PetAbilMeso2, BodyPart.PetAbilMeso3),
                1812001 => ImmutableHashSet.Create(BodyPart.PetAbilItem, BodyPart.PetAbilItem2, BodyPart.PetAbilItem3),
                1812002 => ImmutableHashSet.Create(BodyPart.PetAbilHpConsume),
                1812003 => ImmutableHashSet.Create(BodyPart.PetAbilMpConsume),
                1812004 => ImmutableHashSet.Create(BodyPart.PetAbilSweepForDrop, BodyPart.PetAbilSweepForDrop2, BodyPart.PetAbilSweepForDrop3),
                1812005 => ImmutableHashSet.Create(BodyPart.PetAbilLongRange, BodyPart.PetAbilLongRange2, BodyPart.PetAbilLongRange3),
                1812006 => ImmutableHashSet.Create(BodyPart.PetAbilPickupOthers, BodyPart.PetAbilPickupOthers2, BodyPart.PetAbilPickupOthers3),
                _ => ImmutableHashSet.Create(BodyPart.PetRingLabel, BodyPart.PetRingLabel2, BodyPart.PetRingLabel3)
            },
            182 => ImmutableHashSet.Create(BodyPart.PetRingLabel, BodyPart.PetRingLabel2, BodyPart.PetRingLabel3),
            183 => ImmutableHashSet.Create(BodyPart.PetRingQuote, BodyPart.PetRingQuote2, BodyPart.PetRingQuote3),
            190 => ImmutableHashSet.Create(BodyPart.TamingMob),
            191 => ImmutableHashSet.Create(BodyPart.Saddle),
            192 => ImmutableHashSet.Create(BodyPart.MobEquip),
            194 => ImmutableHashSet.Create(BodyPart.DragonCap),
            195 => ImmutableHashSet.Create(BodyPart.DragonPendant),
            196 => ImmutableHashSet.Create(BodyPart.DragonWing),
            197 => ImmutableHashSet.Create(BodyPart.DragonShoes),
            _ => itemID.IsWeapon()
                ? ImmutableHashSet.Create(BodyPart.Weapon)
                : ImmutableHashSet<BodyPart>.Empty
        };

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
