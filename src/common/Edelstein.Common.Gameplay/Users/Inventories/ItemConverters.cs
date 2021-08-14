using System;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Users.Inventories
{
    public static class ItemConverters
    {
        public static AbstractItemSlot ToItemSlot(this ItemTemplate t) => t switch
        {
            ItemEquipTemplate equip => equip.ToItemSlotEquip(),
            ItemBundleTemplate bundle => bundle.ToItemSlotBundle(),
            ItemPetTemplate pet => pet.ToItemSlotPet(),
            _ => throw new NotImplementedException()
        };

        public static ItemSlotEquip ToItemSlotEquip(this ItemEquipTemplate t)
            => new()
            {
                TemplateID = t.ID,

                RUC = t.TUC,
                STR = t.IncSTR,
                DEX = t.IncDEX,
                INT = t.IncINT,
                LUK = t.IncLUK,
                MaxHP = (short)t.IncMaxHP,
                MaxMP = (short)t.IncMaxMP,
                PAD = t.IncPAD,
                MAD = t.IncMAD,
                PDD = t.IncPDD,
                MDD = t.IncMDD,
                ACC = t.IncACC,
                EVA = t.IncEVA,
                Craft = t.IncCraft,
                Speed = t.IncSpeed,
                Jump = t.IncJump
            };

        public static ItemSlotBundle ToItemSlotBundle(this ItemBundleTemplate t, short number = 1)
            => new()
            {
                TemplateID = t.ID,
                Number = number
            };

        public static ItemSlotPet ToItemSlotPet(this ItemPetTemplate t)
            => new() { TemplateID = t.ID };
    }
}
