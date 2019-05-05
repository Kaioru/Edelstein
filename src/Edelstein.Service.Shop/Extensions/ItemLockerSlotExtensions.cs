using System;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Database.Entities.Inventories.Cash;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Shop.Extensions
{
    public static class ItemLockerSlotExtensions
    {
        public static void Encode(this ItemLockerSlot i, IPacket p)
        {
            p.Encode<long>(i.Item.CashItemSN ?? 0);
            p.Encode<int>(i.AccountID);
            p.Encode<int>(i.CharacterID);
            p.Encode<int>(i.Item.TemplateID);
            p.Encode<int>(i.CommodityID);
            p.Encode<short>((short) (i.Item is ItemSlotBundle bundle
                ? bundle.Number
                : 1));
            p.EncodeFixedString(i.BuyCharacterName, 13);
            p.Encode<DateTime>((i.Item is ItemSlotPet pet
                                   ? pet.DateDead
                                   : i.Item.DateExpire
                               ) ?? ItemConstants.Permanent);
            p.Encode<int>(i.PaybackRate);
            p.Encode<int>(i.DiscountRate);
        }
    }
}