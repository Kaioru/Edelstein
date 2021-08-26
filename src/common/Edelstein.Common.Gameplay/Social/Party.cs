using System.Collections.Generic;
using System.Linq;
using Edelstein.Protocol.Gameplay.Social;
using Edelstein.Protocol.Services.Contracts.Social;

namespace Edelstein.Common.Gameplay.Social
{
    public class Party : IParty
    {
        public int ID { get; }
        public int Boss { get; }
        public ICollection<IPartyMember> Members { get; }

        public Party(PartyContract contract)
        {
            ID = contract.Id;
            Boss = contract.Boss;
            Members = contract.Members.Select<PartyMemberContract, IPartyMember>(m => new PartyMember(m)).ToList();
        }
    }
}
