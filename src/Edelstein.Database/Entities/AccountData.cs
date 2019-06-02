using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Cash;
using Marten.Schema;

namespace Edelstein.Database.Entities
{
    public class AccountData : IDataEntity
    {
        private const string UniqueIndexName = "accountdata_uidx_accountid_worldid";

        public int ID { get; set; }

        [ForeignKey(typeof(Account))]
        [UniqueIndex(IndexType = UniqueIndexType.DuplicatedField, IndexName = UniqueIndexName)]
        public int AccountID { get; set; }

        [UniqueIndex(IndexType = UniqueIndexType.DuplicatedField, IndexName = UniqueIndexName)]
        public byte WorldID { get; set; }

        public int SlotCount { get; set; }

        public ItemLocker Locker { get; set; }
        public ItemTrunk Trunk { get; set; }

        public AccountData()
        {
            Locker = new ItemLocker(999);
            Trunk = new ItemTrunk(4);
        }
    }
}