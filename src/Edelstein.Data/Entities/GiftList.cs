using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities
{
    public class GiftList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public int CharacterID { get; set; }

        public int SN { get; set; }
        public int CommoditySN { get; set; }
        public string BuyCharacterName { get; set; }
        public string Text { get; set; }
    }
}