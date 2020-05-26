using Edelstein.Core.Database;

namespace Edelstein.Core.Entities
{
    public class Account : IDataEntity
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string PIN { get; set; }
        public string SPW { get; set; }

        public byte? Gender { get; set; }

        public byte? LatestConnectedWorld { get; set; }
    }
}