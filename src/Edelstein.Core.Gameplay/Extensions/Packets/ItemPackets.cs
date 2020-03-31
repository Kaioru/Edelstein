using System;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Entities.Inventories.Items;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Extensions.Packets
{
    public static class ItemPackets
    {
        public static void Encode(this ItemSlot i, IPacket p)
        {
            switch (i)
            {
                case ItemSlotEquip equip:
                    equip.Encode(p);
                    break;
                case ItemSlotBundle bundle:
                    bundle.Encode(p);
                    break;
                case ItemSlotPet pet:
                    pet.Encode(p);
                    break;
            }
        }

        public static void EncodeBase(this ItemSlot i, IPacket p)
        {
            p.EncodeInt(i.TemplateID);
            p.EncodeBool(i.CashItemSN.HasValue);
            if (i.CashItemSN.HasValue) p.EncodeLong(i.CashItemSN.Value);
            p.EncodeDateTime(i.DateExpire ?? ItemConstants.Permanent);
        }

        private static void Encode(this ItemSlotEquip i, IPacket p)
        {
            p.EncodeByte(1);

            i.EncodeBase(p);

            p.EncodeByte(i.RUC);
            p.EncodeByte(i.CUC);

            p.EncodeShort(i.STR);
            p.EncodeShort(i.DEX);
            p.EncodeShort(i.INT);
            p.EncodeShort(i.LUK);
            p.EncodeShort(i.MaxHP);
            p.EncodeShort(i.MaxMP);
            p.EncodeShort(i.PAD);
            p.EncodeShort(i.MAD);
            p.EncodeShort(i.PDD);
            p.EncodeShort(i.MDD);
            p.EncodeShort(i.ACC);
            p.EncodeShort(i.EVA);

            p.EncodeShort(i.Craft);
            p.EncodeShort(i.Speed);
            p.EncodeShort(i.Jump);
            p.EncodeString(i.Title);
            p.EncodeShort(i.Attribute);

            p.EncodeByte(i.LevelUpType);
            p.EncodeByte(i.Level);
            p.EncodeInt(i.EXP);
            p.EncodeInt(i.Durability);

            p.EncodeInt(i.IUC);

            p.EncodeByte(i.Grade);
            p.EncodeByte(i.CHUC);

            p.EncodeShort(i.Option1);
            p.EncodeShort(i.Option2);
            p.EncodeShort(i.Option3);
            p.EncodeShort(i.Socket1);
            p.EncodeShort(i.Socket2);

            if (!i.CashItemSN.HasValue) p.EncodeLong(0);
            p.EncodeLong(0);
            p.EncodeInt(0);
        }

        public static void Encode(this ItemSlotBundle i, IPacket p)
        {
            p.EncodeByte(2);

            i.EncodeBase(p);

            p.EncodeShort(i.Number);
            p.EncodeString(i.Title);
            p.EncodeShort(i.Attribute);

            if (ItemConstants.IsRechargeableItem(i.TemplateID))
                p.EncodeLong(0);
        }

        public static void Encode(this ItemSlotPet i, IPacket p)
        {
            p.EncodeByte(3);

            i.EncodeBase(p);

            p.EncodeString(i.PetName, 13);
            p.EncodeByte(i.Level);
            p.EncodeShort(i.Tameness);
            p.EncodeByte(i.Repleteness);

            if (i.DateDead == null) p.EncodeLong(0);
            else p.EncodeDateTime(i.DateDead.Value);

            p.EncodeShort(i.PetAttribute);
            p.EncodeShort(i.PetSkill);
            p.EncodeInt(i.RemainLife);
            p.EncodeShort(i.Attribute);
        }
    }
}