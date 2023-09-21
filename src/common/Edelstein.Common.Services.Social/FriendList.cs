using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Services.Social;

public class FriendList : IFriendList
{
    public FriendList() {}
    public FriendList(IDictionary<int, IFriend> friends) => Records = friends;
    
    public IDictionary<int, IFriend> Records { get; set; }
}
