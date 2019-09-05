using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Store;
using Marten.Schema;

namespace Edelstein.Service.Social.Entities
{
    public class PartyRecord : IDataEntity
    {
        public int ID { get; set; }
        
        [ForeignKey(typeof(Character))] public int BossCharacterID { get; set; }
    }
}