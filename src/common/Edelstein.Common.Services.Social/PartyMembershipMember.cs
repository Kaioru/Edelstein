using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Services.Social;

public record PartyMembershipMember : IPartyMember
{
    public int PartyID { get; set; }
    
    public int CharacterID { get; set; }
    public string CharacterName { get; set; }
    
    public int Job { get; set; }
    public int Level { get; set; }
    
    public int ChannelID { get; set; }
    public int FieldID { get; set; }

    public PartyMembershipMember() {}
    public PartyMembershipMember(IPartyMember member)
    {
        PartyID = member.PartyID;
        CharacterID = member.CharacterID;
        CharacterName = member.CharacterName;
        Job = member.Job;
        Level = member.Level;
        ChannelID = member.ChannelID;
        FieldID = member.FieldID;
    }
}
