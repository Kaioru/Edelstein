using System;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Users.Inventories
{
    public static class ItemPackets
    {
        public static void WriteItemData(this IPacketWriter p, AbstractItemSlot i)
        {
            switch (i)
            {
                case ItemSlotEquip equip:
                    p.WriteItemEquipData(equip);
                    break;
                case ItemSlotBundle bundle:
                    p.WriteItemBundleData(bundle);
                    break;
                case ItemSlotPet pet:
                    p.WriteItemPetData(pet);
                    break;
            }
        }

        private static void WriteItemBase(this IPacketWriter p, AbstractItemSlot i)
        {
            p.WriteInt(i.TemplateID);
            p.WriteBool(i.CashItemSN.HasValue);
            if (i.CashItemSN.HasValue) p.WriteLong(i.CashItemSN.Value);
            p.WriteDateTime(i.DateExpire ?? GameConstants.Permanent); // TODO: constants
        }

        private static void WriteItemEquipData(this IPacketWriter p, ItemSlotEquip i)
        {
            p.WriteByte(1);

            p.WriteItemBase(i);

            p.WriteByte(i.RUC);
            p.WriteByte(i.CUC);

            p.WriteShort(i.STR);
            p.WriteShort(i.DEX);
            p.WriteShort(i.INT);
            p.WriteShort(i.LUK);
            p.WriteShort(i.MaxHP);
            p.WriteShort(i.MaxMP);
            p.WriteShort(i.PAD);
            p.WriteShort(i.MAD);
            p.WriteShort(i.PDD);
            p.WriteShort(i.MDD);
            p.WriteShort(i.ACC);
            p.WriteShort(i.EVA);

            p.WriteShort(i.Craft);
            p.WriteShort(i.Speed);
            p.WriteShort(i.Jump);
            p.WriteString(i.Title);
            p.WriteShort(i.Attribute);

            p.WriteByte(i.LevelUpType);
            p.WriteByte(i.Level);
            p.WriteInt(i.EXP);
            p.WriteInt(i.Durability);

            p.WriteInt(i.IUC);

            p.WriteByte(i.Grade);
            p.WriteByte(i.CHUC);

            p.WriteShort(i.Option1);
            p.WriteShort(i.Option2);
            p.WriteShort(i.Option3);
            p.WriteShort(i.Socket1);
            p.WriteShort(i.Socket2);

            if (!i.CashItemSN.HasValue) p.WriteLong(0);
            p.WriteLong(0);
            p.WriteInt(0);
        }

        private static void WriteItemBundleData(this IPacketWriter p, ItemSlotBundle i)
        {
            p.WriteByte(2);

            p.WriteItemBase(i);

            p.WriteShort(i.Number);
            p.WriteString(i.Title);
            p.WriteShort(i.Attribute);

            if ((i.TemplateID / 10000) == 207 || (i.TemplateID / 10000) == 233) // TODO: constants, recharge items
                p.WriteLong(0);
        }

        private static void WriteItemPetData(this IPacketWriter p, ItemSlotPet i)
        {
            p.WriteByte(3);

            p.WriteItemBase(i);

            p.WriteString(i.PetName, 13);
            p.WriteByte(i.Level);
            p.WriteShort(i.Tameness);
            p.WriteByte(i.Repleteness);

            if (i.DateDead == null) p.WriteLong(0);
            else p.WriteDateTime(i.DateDead.Value);

            p.WriteShort(i.PetAttribute);
            p.WriteShort(i.PetSkill);
            p.WriteInt(i.RemainLife);
            p.WriteShort(i.Attribute);
        }
    }
}
