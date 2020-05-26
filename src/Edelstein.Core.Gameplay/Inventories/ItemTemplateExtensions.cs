using System;
using Edelstein.Core.Entities.Inventories.Items;
using Edelstein.Core.Gameplay.Utils;
using Edelstein.Core.Templates.Items;
using Edelstein.Core.Templates.Items.Cash;

namespace Edelstein.Core.Gameplay.Inventories
{
    public static class ItemTemplateExtensions
    {
        public static ItemSlot ToItemSlot(
            this ItemTemplate template,
            ItemVariationType type = ItemVariationType.None
        )
        {
            return template switch
            {
                ItemEquipTemplate equipTemplate => (ItemSlot) equipTemplate.ToItemSlot(type),
                ItemBundleTemplate bundleTemplate => bundleTemplate.ToItemSlot(),
                PetItemTemplate petTemplate => petTemplate.ToItemSlot(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static ItemSlotEquip ToItemSlot(
            this ItemEquipTemplate template,
            ItemVariationType type = ItemVariationType.None
        )
        {
            var variation = new ItemVariation(Rand32.Create(), type);
            return new ItemSlotEquip
            {
                TemplateID = template.ID,

                RUC = template.TUC,
                STR = (short) variation.Get(template.IncSTR),
                DEX = (short) variation.Get(template.IncDEX),
                INT = (short) variation.Get(template.IncINT),
                LUK = (short) variation.Get(template.IncLUK),
                MaxHP = (short) variation.Get(template.IncMaxHP),
                MaxMP = (short) variation.Get(template.IncMaxMP),
                PAD = (short) variation.Get(template.IncPAD),
                MAD = (short) variation.Get(template.IncMAD),
                PDD = (short) variation.Get(template.IncPDD),
                MDD = (short) variation.Get(template.IncMDD),
                ACC = (short) variation.Get(template.IncACC),
                EVA = (short) variation.Get(template.IncEVA),
                Craft = (short) variation.Get(template.IncCraft),
                Speed = (short) variation.Get(template.IncSpeed),
                Jump = (short) variation.Get(template.IncJump),
                Durability = 100
            };
        }

        private static ItemSlotBundle ToItemSlot(this ItemBundleTemplate template)
        {
            return new ItemSlotBundle
            {
                TemplateID = template.ID,
                Number = 1,
                MaxNumber = template.MaxPerSlot
            };
        }

        private static ItemSlotPet ToItemSlot(this PetItemTemplate template)
        {
            var i = new ItemSlotPet
            {
                TemplateID = template.ID
            };

            if (template.Life > 0)
                i.DateDead = DateTime.UtcNow.AddDays(template.Life);

            return i;
        }
    }
}