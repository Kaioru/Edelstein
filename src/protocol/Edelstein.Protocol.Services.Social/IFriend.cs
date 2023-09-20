using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Services.Social;

public interface IFriend : IIdentifiable<int>
{
    int FriendID { get; }
    string FriendName { get; }
    string FriendGroup { get; set; }
    
    short Flag { get; set; }
    
    int ChannelID { get; set; }
}
