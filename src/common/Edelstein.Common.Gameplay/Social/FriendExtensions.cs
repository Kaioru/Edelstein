using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Social;

public static class FriendExtensions
{
    public static void WriteFriendInfo(this IPacketWriter writer, IFriend friend)
    {
        writer.WriteInt(friend.FriendID);
        writer.WriteString(friend.FriendName, 13);
        writer.WriteByte((byte)friend.Flag);
        writer.WriteInt(friend.ChannelID);
        writer.WriteString(friend.FriendGroup, 17);
    }
}
