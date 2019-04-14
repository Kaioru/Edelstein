using Marten.Schema;

namespace Edelstein.Database
{
    public class AccountData
    {
        private const string UniqueIndexName = "accountdata_uidx_accountid_worldid";
        
        public int ID { get; set; }

        [ForeignKey(typeof(Account))]
        [UniqueIndex(IndexType = UniqueIndexType.DuplicatedField, IndexName = UniqueIndexName)]
        public int AccountID { get; set; }

        [UniqueIndex(IndexType = UniqueIndexType.DuplicatedField, IndexName = UniqueIndexName)]
        public byte WorldID { get; set; }

        public int SlotCount { get; set; }
    }
}