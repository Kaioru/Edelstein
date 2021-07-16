using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public class AccountWorld : IRepositoryEntry<int>
    {
        public int ID { get; init; }
        public int AccountID { get; init; }
        public byte WorldID { get; init; }

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
