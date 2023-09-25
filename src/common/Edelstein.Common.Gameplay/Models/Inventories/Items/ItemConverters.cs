using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Models.Inventories.Items;

public static class ItemConverters
{
    public static IItemSlot ToItemSlot(this IItemTemplate template, ItemVariationOption option = ItemVariationOption.None) => template switch
    {
        IItemEquipTemplate equip => equip.ToItemSlotEquip(),
        IItemBundleTemplate bundle => bundle.ToItemSlotBundle(),
        IItemPetTemplate pet => pet.ToItemSlotPet(),
        _ => new ItemSlot { ID = template.ID }
    };

    public static IItemSlotEquip ToItemSlotEquip(this IItemEquipTemplate template, ItemVariationOption option = ItemVariationOption.None)
        => new ItemSlotEquip
        {
            ID = template.ID,

            RUC = template.TUC,
            STR = (short)GetVariation(template.IncSTR, option),
            DEX = (short)GetVariation(template.IncDEX, option),
            INT = (short)GetVariation(template.IncINT, option),
            LUK = (short)GetVariation(template.IncLUK, option),
            MaxHP = (short)GetVariation(template.IncMaxHP, option),
            MaxMP = (short)GetVariation(template.IncMaxMP, option),
            PAD = (short)GetVariation(template.IncPAD, option),
            MAD = (short)GetVariation(template.IncMAD, option),
            PDD = (short)GetVariation(template.IncPDD, option),
            MDD = (short)GetVariation(template.IncMDD, option),
            ACC = (short)GetVariation(template.IncACC, option),
            EVA = (short)GetVariation(template.IncEVA, option),
            Craft = (short)GetVariation(template.IncCraft, option),
            Speed = (short)GetVariation(template.IncSpeed, option),
            Jump = (short)GetVariation(template.IncJump, option)
        };

    public static IItemSlotBundle ToItemSlotBundle(this IItemBundleTemplate template, short count = 1)
        => new ItemSlotBundle
        {
            ID = template.ID,
            Number = count
        };

    public static IItemSlotPet ToItemSlotPet(this IItemPetTemplate template)
        => new ItemSlotPet { ID = template.ID };

    public static int GetVariation(int value, ItemVariationOption option)
    {
        if (value <= 0) return value;
        if (option == ItemVariationOption.None) return value;
        var rand = new Random();
        if (option != ItemVariationOption.Gachapon)
        {
            var v9 = value / 10 + 1;
            if (v9 >= 5)
                v9 = 5;
            var v10 = 1 << v9 + 2;
            var v11 = rand.Next(v10);
            var v12 = (byte)v11;
            var v13 = v11 >> 1;
            var v14 = (v13 >> 3 & 1) +
                      (v13 >> 2 & 1) +
                      (v13 >> 1 & 1) +
                      (v13 & 1) +
                      (v12 & 1) - 2 +
                      (v13 >> 4 & 1) +
                      (v13 >> 5 & 1);
            if (v14 <= 0)
                v14 = 0;
            if (option == ItemVariationOption.Normal)
            {
                var v15 = (rand.Next() & 1) == 0;
                if (v15)
                    return value - v14;
            }
            else
            {
                var v16 = rand.Next(10) < 3;
                switch (option)
                {
                    case ItemVariationOption.Better when v16:
                        return value;
                    case ItemVariationOption.Great when v16:
                        return value;
                }
            }
            return value + v14;
        }

        var v3 = value / 5 + 1;
        if (v3 >= 7)
            v3 = 7;
        var v4 = 1 << v3 + 2;
        var enOptiona = (ItemVariationOption)(v3 + 2);
        var v5 = v4 > 0 ? rand.Next(v4) : rand.Next();
        var v6 = (int)enOptiona;
        var v7 = -2;
        if (enOptiona > ItemVariationOption.None)
        {
            do
            {
                v7 += v5 & 1;
                v5 >>= 1;
                v6--;
            }
            while (v6 > 0);
        }
        if ((rand.Next() & 1) != 0)
            return value + v7;
        return value - v7;
    }
}
