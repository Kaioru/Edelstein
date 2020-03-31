using System;
using Edelstein.Database;

namespace Edelstein.Entities.Social
{
    public class Memo : IDataEntity
    {
        public int ID { get; set; }
        public int CharacterID { get; set; }

        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; }
        public byte Flag { get; set; }
    }
}