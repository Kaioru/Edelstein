using Edelstein.Database.Entities.Inventories.Items;
using Marten.Schema;

namespace Edelstein.Database.Entities
{
    public class GiftRecord : IDataEntity
    {
        public int ID { get; set; }

        [ForeignKey(typeof(AccountData))] public int AccountDataID { get; set; }

        public ItemSlot Item { get; set; }
        public string BuyCharacterName { get; set; }
        public string Text { get; set; }
    }
}