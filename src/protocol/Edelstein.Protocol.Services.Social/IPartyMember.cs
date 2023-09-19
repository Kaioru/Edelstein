namespace Edelstein.Protocol.Services.Social;

public interface IPartyMember
{
    int PartyID { get; }
    
    int CharacterID { get; }
    int CharacterName { get; }
    
    int Job { get; }
    int Level { get; }
    
    int ChannelID { get; }
}
