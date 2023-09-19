using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Services.Social.Entities;

public class PartyEntity : IParty
{
    public int ID { get; set; }
    
    public int BossCharacterID { get; set; }

    public ICollection<PartyMemberEntity> Members { get; set; } = new List<PartyMemberEntity>();
}
