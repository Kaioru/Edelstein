using Marten.Schema;

namespace Edelstein.Database.Entities
{
    public class Account
    {
        public int ID { get; set; }

        [UniqueIndex(IndexType = UniqueIndexType.Computed)]
        public string Username { get; set; }

        public string Password { get; set; }
        public string SecondPassword { get; set; }

        public byte? Gender { get; set; }
        public int NexonCash { get; set; }
        public int MaplePoint { get; set; }
        public int PrepaidNXCash { get; set; }

        public byte LatestConnectedWorld { get; set; }
        public string PreviousConnectedService { get; set; }

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