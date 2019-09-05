using System.Collections.Generic;

namespace Edelstein.Core.Gameplay.Social
{
    public class PartyData
    {
        public int ID { get; set; }
        public int BossCharacterID { get; set; }
        public List<PartyMemberData> Members { get; set; }
    }
}