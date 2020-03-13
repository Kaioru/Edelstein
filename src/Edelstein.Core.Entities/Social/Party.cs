using Edelstein.Database;

namespace Edelstein.Entities.Social
{
    public class Party : IDataEntity
    {
        public int ID { get; set; }
        
        public int BossCharacterID { get; set; }
    }
}