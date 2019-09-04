using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Store;
using Marten.Schema;

namespace Edelstein.Database.Entities
{
    public class RankRecord : IDataEntity
    {
        public int ID { get; set; }

        [ForeignKey(typeof(Character))] public int CharacterID { get; set; }
        
        public int WorldRank { get; set; }
        public int WorldRankGap { get; set; }
        public int JobRank { get; set; }
        public int JobRankGap { get; set; }
    }
}