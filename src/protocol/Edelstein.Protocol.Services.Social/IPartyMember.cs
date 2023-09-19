namespace Edelstein.Protocol.Services.Social;

public interface IPartyMember
{
    int PartyID { get; }
    
    int CharacterID { get; }
    string CharacterName { get; }
    
    int Job { get; set; }
    int Level { get; set; }
    
    int ChannelID { get; set; }
    int FieldID { get; set; }
}
