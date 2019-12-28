using Edelstein.Database;

namespace Edelstein.Entities
{
    public class Character : IDataEntity
    {
        public int ID { get; set; }

        public int AccountWorldID { get; set; }

        public string Name { get; set; }
    }
}