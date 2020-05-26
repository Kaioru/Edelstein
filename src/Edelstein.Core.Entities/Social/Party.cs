using Edelstein.Core.Database;

namespace Edelstein.Core.Entities.Social
{
    public class Party : IDataEntity
    {
        public int ID { get; set; }

        public int BossCharacterID { get; set; }
    }
}