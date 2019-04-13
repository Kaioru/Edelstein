using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(13)] public string Username { get; set; }
        [MaxLength(128)] public string Password { get; set; }
        [MaxLength(128)] public string SecondPassword { get; set; }

        public byte? Gender { get; set; }
        public int NexonCash { get; set; }
        public int MaplePoint { get; set; }
        public int PrepaidNXCash { get; set; }

        public byte LatestConnectedWorld { get; set; }
        public string LatestConnectedService { get; set; }
        public string PreviousConnectedService { get; set; }

        public ICollection<AccountData> Data { get; set; }
    }
}