using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Social.Memo
{
    public static class SocialMemoPackets
    {
        public static void EncodeData(this ISocialMemo memo, IPacket p)
        {
            p.EncodeInt(memo.ID);
            p.EncodeString(memo.Sender);
            p.EncodeString(memo.Content);
            p.EncodeDateTime(memo.DateSent);
            p.EncodeByte(memo.Flag);
        }
    }
}