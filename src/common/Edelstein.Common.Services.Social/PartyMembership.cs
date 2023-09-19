using Edelstein.Common.Services.Social.Entities;
using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Services.Social;

public class PartyMembership : IPartyMembership
{
    public int ID { get; }
    
    public int BossCharacterID { get; }

    public int PartyID => ID;
    
    public int CharacterID { get; }
    public string CharacterName { get; }
    
    public int Job { get; }
    public int Level { get; }
    
    public int ChannelID { get; }
    public int FieldID { get; }

    public IDictionary<int, IPartyMember> Members { get; }

    public PartyMembership(PartyMemberEntity partyMember)
    {
        ID = partyMember.Party.ID;
        BossCharacterID = partyMember.Party.BossCharacterID;
        CharacterID = partyMember.CharacterID;
        CharacterName = partyMember.CharacterName;
        Job = partyMember.Job;
        Level = partyMember.Level;
        ChannelID = partyMember.ChannelID;
        FieldID = partyMember.FieldID;
        Members = partyMember.Party.Members.ToDictionary(m => m.CharacterID, m => (IPartyMember)m);
    }
}
