using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Services.Social;

public interface IFriend : IIdentifiable<int>
{
    int FriendID { get; }
    string FriendName { get; }
    string FriendGroup { get; }
    
    short Flag { get; }
    
    int ChannelID { get; }
}
