using Edelstein.Core.Database;

namespace Edelstein.Core.Entities.Social
{
    public class PartyMember : IDataEntity
    {
        public int ID { get; set; }
        public int PartyID { get; set; }

        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public int Job { get; set; }
        public int Level { get; set; }
        public int ChannelID { get; set; }
        public int FieldID { get; set; }
    }
}