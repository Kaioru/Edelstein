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

        public AccountGradeCode GradeCode { get; set; }
        public AccountSubGradeCode SubGradeCode { get; set; }

        public byte? Gender { get; set; }

        public byte? LatestConnectedWorld { get; set; }

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }
    }
}
