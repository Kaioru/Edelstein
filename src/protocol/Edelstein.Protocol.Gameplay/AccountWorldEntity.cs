using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay
{
    public class AccountWorldEntity : IRepositoryEntry<int>
    {
        public int ID { get; init; }
        public int AccountID { get; init; }
        public byte WorldID { get; init; }

        public int SlotCount { get; set; }

        public ItemLocker Locker { get; set; }
        public ItemTrunk Trunk { get; set; }

        public AccountWorldEntity()
        {
            SlotCount = 3;
            Locker = new ItemLocker(999);
            Trunk = new ItemTrunk(4);
        }
    }
}
