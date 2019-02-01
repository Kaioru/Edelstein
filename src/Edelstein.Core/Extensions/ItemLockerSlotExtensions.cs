using System;
using Edelstein.Core.Constants;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Network.Packet;

namespace Edelstein.Core.Extensions
{
    public static class ItemLockerSlotExtensions
    {
        public static void Encode(this ItemLockerSlot i, IPacket p)
        {
            p.Encode<long>(i.SN);
            p.Encode<int>(0);
            p.Encode<int>(0);
            p.Encode<int>(i.ItemID);
            p.Encode<int>(i.CommoditySN);
            p.Encode<short>(i.Number);
            p.EncodeFixedString(i.BuyCharacterName ?? string.Empty, 13);
            p.Encode<DateTime>(i.DateExpire ?? ItemConstants.Permanent);
            p.Encode<int>(i.PaybackRate);
            p.Encode<int>(i.DiscountRate);
        }
    }
}