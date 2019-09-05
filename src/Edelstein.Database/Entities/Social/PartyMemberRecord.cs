using Edelstein.Database.Entities.Characters;
using Edelstein.Database.Store;
using Marten.Schema;

namespace Edelstein.Database.Entities.Social
{
    public class PartyMemberRecord : IDataEntity
    {
        public int ID { get; set; }
        
        [ForeignKey(typeof(PartyRecord))] public int PartyRecordID { get; set; }
        [ForeignKey(typeof(Character))] public int CharacterID { get; set; }

        public string Name { get; set; }
        public short Job { get; set; }
        public byte Level { get; set; }
        public int ChannelID { get; set; }
    }
}