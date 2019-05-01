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
    }
}