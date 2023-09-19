using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Social.Entities;

public record PartyMemberEntity : IPartyMember, IIdentifiable<int>
{
    public int ID { get; set; }
    public int PartyID { get; set; }
    
    public PartyEntity Party { get; set; }
    
    public int CharacterID { get; set; }
    public string CharacterName { get; set; }
    
    public int Job { get; set; }
    public int Level { get; set; }
    
    public int ChannelID { get; set; }
    public int FieldID { get; set; }
}
