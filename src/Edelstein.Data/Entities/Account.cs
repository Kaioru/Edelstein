using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Data.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(13)] public string Username { get; set; }
        [MaxLength(128)] public string Password { get; set; }
        [MaxLength(128)] public string SecondPassword { get; set; }
        
        public byte LatestConnectedWorld { get; set; }
        public string LatestConnectedService { get; set; }
        public string PreviousConnectedService { get; set; }

        public ICollection<AccountData> Data { get; set; }
    }
}