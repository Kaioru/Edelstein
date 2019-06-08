using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Store;
using Marten.Schema;

namespace Edelstein.Database.Entities
{
    public class CoupleRecord : IDataEntity
    {
        public int ID { get; set; }
        
        [ForeignKey(typeof(Character))] public int CharacterID { get; set; } 
        [ForeignKey(typeof(Character))] public int PairCharacterID { get; set; } 
        
        public string PairCharacterName { get; set; }
        public long SN { get; set; }
        public long PairSN { get; set; }
    }
}