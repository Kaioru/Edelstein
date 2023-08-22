using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items;

public static class ItemConverters
{
    public static IItemSlot ToItemSlot(this IItemTemplate template) => template switch
    {
        IItemEquipTemplate equip => equip.ToItemSlotEquip(),
        IItemBundleTemplate bundle => bundle.ToItemSlotBundle(),
        IItemPetTemplate pet => pet.ToItemSlotPet(),
        _ => new ItemSlot { ID = template.ID }
    };

    public static IItemSlotEquip ToItemSlotEquip(this IItemEquipTemplate template)
        => new ItemSlotEquip
        {
            ID = template.ID,

            RUC = template.TUC,
            STR = template.IncSTR,
            DEX = template.IncDEX,
            INT = template.IncINT,
            LUK = template.IncLUK,
            MaxHP = (short)template.IncMaxHP,
            MaxMP = (short)template.IncMaxMP,
            PAD = template.IncPAD,
            MAD = template.IncMAD,
            PDD = template.IncPDD,
            MDD = template.IncMDD,
            ACC = template.IncACC,
            EVA = template.IncEVA,
            Craft = template.IncCraft,
            Speed = template.IncSpeed,
            Jump = template.IncJump
        };

    public static IItemSlotBundle ToItemSlotBundle(this IItemBundleTemplate template, short count = 1)
        => new ItemSlotBundle
        {
            ID = template.ID,
            Number = count
        };

    public static IItemSlotPet ToItemSlotPet(this IItemPetTemplate template)
        => new ItemSlotPet { ID = template.ID };
}
