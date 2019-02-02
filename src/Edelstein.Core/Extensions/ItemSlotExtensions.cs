using System;
using Edelstein.Core.Constants;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;

namespace Edelstein.Core.Extensions
{
    public static class ItemSlotExtensions
    {
        public static void EncodeBase(this ItemSlot i, IPacket p)
        {
            p.Encode<int>(i.TemplateID);
            p.Encode<bool>(i.CashItemSN.HasValue);
            if (i.CashItemSN.HasValue) p.Encode<long>(i.CashItemSN.Value);
            p.Encode<DateTime>(i.DateExpire ?? ItemConstants.Permanent);
        }

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

        private static void Encode(this ItemSlotEquip i, IPacket p)
        {
            p.Encode<byte>(1);

            i.EncodeBase(p);

            p.Encode<byte>(i.RUC);
            p.Encode<byte>(i.CUC);

            p.Encode<short>(i.STR);
            p.Encode<short>(i.DEX);
            p.Encode<short>(i.INT);
            p.Encode<short>(i.LUK);
            p.Encode<short>(i.MaxHP);
            p.Encode<short>(i.MaxMP);
            p.Encode<short>(i.PAD);
            p.Encode<short>(i.MAD);
            p.Encode<short>(i.PDD);
            p.Encode<short>(i.MDD);
            p.Encode<short>(i.ACC);
            p.Encode<short>(i.EVA);

            p.Encode<short>(i.Craft);
            p.Encode<short>(i.Speed);
            p.Encode<short>(i.Jump);
            p.Encode<string>(i.Title);
            p.Encode<short>(i.Attribute);

            p.Encode<byte>(i.LevelUpType);
            p.Encode<byte>(i.Level);
            p.Encode<int>(i.EXP);
            p.Encode<int>(i.Durability);

            p.Encode<int>(i.IUC);

            p.Encode<byte>(i.Grade);
            p.Encode<byte>(i.CHUC);

            p.Encode<short>(i.Option1);
            p.Encode<short>(i.Option2);
            p.Encode<short>(i.Option3);
            p.Encode<short>(i.Socket1);
            p.Encode<short>(i.Socket2);

            if (!i.CashItemSN.HasValue) p.Encode<long>(0);
            p.Encode<long>(0);
            p.Encode<int>(0);
        }

        public static void Encode(this ItemSlotBundle i, IPacket p)
        {
            p.Encode<byte>(2);

            i.EncodeBase(p);

            p.Encode<short>(i.Number);
            p.Encode<string>(i.Title);
            p.Encode<short>(i.Attribute);

            if (ItemConstants.IsRechargeableItem(i.TemplateID))
                p.Encode<long>(0);
        }

        public static void Encode(this ItemSlotPet i, IPacket p)
        {
            p.Encode<byte>(3);

            i.EncodeBase(p);

            p.EncodeFixedString(i.PetName, 13);
            p.Encode<byte>(i.Level);
            p.Encode<short>(i.Tameness);
            p.Encode<byte>(i.Repleteness);

            if (i.DateDead == null) p.Encode<long>(0);
            else p.Encode<DateTime>(i.DateDead.Value);

            p.Encode<short>(i.PetAttribute);
            p.Encode<short>(i.PetSkill);
            p.Encode<int>(i.RemainLife);
            p.Encode<short>(i.Attribute);
        }

        public static void EncodeLocker(this ItemSlot i, IPacket p)
        {
            p.Encode<long>(i.CashItemSN ?? 0);
            p.Encode<int>(0);
            p.Encode<int>(0);
            p.Encode<int>(i.TemplateID);
            p.Encode<int>(0);
            p.Encode<short>((short) (i is ItemSlotBundle bundle
                ? bundle.Number
                : 1));
            p.EncodeFixedString(string.Empty, 13);
            p.Encode<DateTime>((i is ItemSlotPet pet
                                   ? pet.DateDead
                                   : i.DateExpire
                               ) ?? ItemConstants.Permanent);
            p.Encode<int>(0);
            p.Encode<int>(0);
        }
    }
}