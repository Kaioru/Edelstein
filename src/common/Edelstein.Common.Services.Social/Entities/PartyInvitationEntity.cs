using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Social.Entities;

public record PartyInvitationEntity : IIdentifiable<int>
{
    public int ID { get; set; }
    public int PartyID { get; set; }
        
    public PartyEntity Party { get; set; }
    
    public int CharacterID { get; set; }
    public DateTime DateExpire { get; set; }
}
