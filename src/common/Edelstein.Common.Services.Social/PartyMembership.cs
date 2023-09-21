using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Services.Social;

public class PartyMembership : IPartyMembership
{
    public int ID { get; set; }
    
    public int BossCharacterID { get; set; }

    public int PartyID { get; set; }
    
    public int CharacterID { get; set; }
    public string CharacterName { get; set; }
    
    public int Job { get; set; }
    public int Level { get; set; }
    
    public int ChannelID { get; set; }
    public int FieldID { get; set; }

    public IDictionary<int, IPartyMember> Members { get; set; }

    public PartyMembership() {}
    public PartyMembership(PartyMemberEntity partyMember)
    {
        ID = partyMember.Party.ID;
        PartyID = partyMember.Party.ID;
        BossCharacterID = partyMember.Party.BossCharacterID;
        CharacterID = partyMember.CharacterID;
        CharacterName = partyMember.CharacterName;
        Job = partyMember.Job;
        Level = partyMember.Level;
        ChannelID = partyMember.ChannelID;
        FieldID = partyMember.FieldID;
        Members = partyMember.Party.Members
            .ToDictionary(
                m => m.CharacterID, 
                m => (IPartyMember)new PartyMembershipMember(m));
    }
}
