using Edelstein.Core.Constants;
using Edelstein.Core.Utils;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Provider.Templates.Item;

namespace Edelstein.Core.Extensions
{
    public static class ItemTemplateExtensions
    {
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
                MaxNumber = template.MaxPerSlot
            };
        }

        public static ItemSlot ToItemSlot(
            this ItemTemplate template,
            ItemVariationType type = ItemVariationType.None
        )
        {
            switch (template)
            {
                case ItemEquipTemplate equipTemplate:
                    return equipTemplate.ToItemSlot(type);
                case ItemBundleTemplate bundleTemplate:
                    return bundleTemplate.ToItemSlot();
            }

            return null;
        }
    }
}