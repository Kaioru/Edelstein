using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Store;
using Marten.Schema;

namespace Edelstein.Database.Entities.Social
{
    public class PartyRecord : IDataEntity
    {
        public int ID { get; set; }
        
        [ForeignKey(typeof(Character))] public int BossCharacterID { get; set; }
    }
}