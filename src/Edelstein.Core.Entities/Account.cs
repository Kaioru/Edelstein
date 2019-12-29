using Edelstein.Database;

namespace Edelstein.Entities
{
    public class Account : IDataEntity
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string PIN { get; set; }
        public string PIC { get; set; }

        public byte? Gender { get; set; }
    }
}