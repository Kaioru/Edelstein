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

        
        public byte? Gender { get; set; }
        public int NexonCash { get; set; }
        public int MaplePoint { get; set; }
        public int PrepaidNXCash { get; set; }

        public byte LatestConnectedWorld { get; set; }
        public string LatestConnectedService { get; set; }
        public string PreviousConnectedService { get; set; }

        public ICollection<AccountData> Data { get; set; }

        public int GetCash(int type)
        {
            switch (type)
            {
                default:
                case 0x1: return NexonCash;
                case 0x2: return MaplePoint;
                case 0x4: return PrepaidNXCash;
            }
        }

        public void IncCash(int type, int amount)
        {
            switch (type)
            {
                default:
                case 0x1:
                    NexonCash += amount;
                    break;
                case 0x2:
                    MaplePoint += amount;
                    break;
                case 0x4:
                    PrepaidNXCash += amount;
                    break;
            }
        }
    }
}