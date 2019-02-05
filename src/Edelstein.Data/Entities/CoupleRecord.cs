using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities
{
    public class CoupleRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public int CharacterID { get; set; }
        public int PairCharacterID { get; set; }
        public string PairCharacterName { get; set; }
        public long SN { get; set; }
        public long PairSN { get; set; }
    }
}