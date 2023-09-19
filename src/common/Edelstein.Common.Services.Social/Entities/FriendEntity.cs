using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Services.Social.Entities;

public class FriendEntity : IFriend
{
    public int ID { get; }
    
    public int CharacterID { get; set; }
    
    public int FriendID { get; set; }
    public string FriendName { get; set; }
    public string FriendGroup { get; set; }
    
    public short Flag { get; set; }
    
    public int ChannelID { get; set; }
}
