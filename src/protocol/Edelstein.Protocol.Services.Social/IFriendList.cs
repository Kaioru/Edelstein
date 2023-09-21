namespace Edelstein.Protocol.Services.Social;

public interface IFriendList
{
    IDictionary<int, IFriend> Records { get; }
}
