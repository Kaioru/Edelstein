using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities
{
    public class Memo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public int CharacterID { get; set; }
        
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
        public MemoType Flag { get; set; }
    }
}