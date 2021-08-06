using System;
using Edelstein.Protocol.Datastore;

namespace Edelstein.Protocol.Gameplay.Users
{
    public record Account : IDataDocument
    {
        public int ID { get; init; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string PIN { get; set; }
        public string SPW { get; set; }
        public bool Banned { get; set; }
        public byte BlockReason { get; set; }
        public DateTime DateBlocked { get; set; }
        public DateTime DateUnblocked { get; set; }

        public AccountGradeCode GradeCode { get; set; }
        public AccountSubGradeCode SubGradeCode { get; set; }

        public byte? Gender { get; set; }

        public byte? LatestConnectedWorld { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
