using System.Collections.Generic;
using Edelstein.Database;

namespace Edelstein.Entities.Social
{
    public class Party : IDataEntity
    {
        public int ID { get; set; }
        
        public int BossCharacterID { get; set; }
        public ICollection<PartyMember> Members { get; set; }

        public Party()
        {
            Members = new List<PartyMember>();
        }
    }
}