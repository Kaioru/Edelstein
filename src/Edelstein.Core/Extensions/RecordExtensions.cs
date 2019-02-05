using Edelstein.Data.Entities;
using Edelstein.Network.Packet;

namespace Edelstein.Core.Extensions
{
    public static class RecordExtensions
    {
        public static void Encode(this CoupleRecord c, IPacket p)
        {
            p.Encode<int>(c.PairCharacterID);
            p.EncodeFixedString(c.PairCharacterName, 13);
            p.Encode<long>(c.SN);
            p.Encode<long>(c.PairSN);
        }

        public static void Encode(this FriendRecord f, IPacket p)
        {
            p.Encode<int>(f.PairCharacterID);
            p.EncodeFixedString(f.PairCharacterName, 13);
            p.Encode<long>(f.SN);
            p.Encode<long>(f.PairSN);
            p.Encode<int>(f.FriendItemID);
        }
    }
}