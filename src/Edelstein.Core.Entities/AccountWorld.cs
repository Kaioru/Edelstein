using Edelstein.Database;
using Edelstein.Entities.Inventories;
using Edelstein.Entities.Inventories.Cash;

namespace Edelstein.Entities
{
    public class AccountWorld : IDataEntity
    {
        public int ID { get; set; }

        public int AccountID { get; set; }
        public byte WorldID { get; set; }

        public int SlotCount { get; set; }

        public ItemLocker Locker { get; set; }
        public ItemTrunk Trunk { get; set; }

        public AccountWorld()
        {
            SlotCount = 3;
            Locker = new ItemLocker(999);
            Trunk = new ItemTrunk(4);
        }
    }
}