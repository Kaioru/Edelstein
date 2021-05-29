using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay
{
    public class AccountEntity : IRepositoryEntry<int>
    {
        public int ID { get; init; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string PIN { get; set; }
        public string SPW { get; set; }

        public byte? Gender { get; set; }

        public byte? LatestConnectedWorld { get; set; }
    }
}
