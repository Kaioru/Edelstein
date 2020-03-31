using System;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Gameplay.Social.Memo
{
    public static class SocialMemoPackets
    {
        public static void EncodeData(this ISocialMemo memo, IPacket p)
        {
            p.Encode<int>(memo.ID);
            p.Encode<string>(memo.Sender);
            p.Encode<string>(memo.Content);
            p.Encode<DateTime>(memo.DateSent);
            p.Encode<byte>(memo.Flag);
        }
    }
}