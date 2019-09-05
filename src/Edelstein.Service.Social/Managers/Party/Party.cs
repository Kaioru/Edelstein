using System.Collections.Generic;
using Edelstein.Service.Social.Entities;

namespace Edelstein.Service.Social.Managers.Party
{
    public class Party
    {
        public PartyRecord Record { get; set; }

        public PartyMember Boss { get; set; }
        public List<PartyMember> Members { get; set; }
    }
}