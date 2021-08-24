using System;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Users.Inventories;

namespace Edelstein.Protocol.Gameplay.Users
{
    public record AccountWorld : IDataDocument
    {
        public int ID { get; init; }
        public int AccountID { get; init; }
        public byte WorldID { get; init; }

        public int SlotCount { get; set; }

        public ItemLocker Locker { get; set; }
        public ItemTrunk Trunk { get; set; }

        public DateTime DateDocumentCreated { get; set; }
        public DateTime DateDocumentUpdated { get; set; }

        public AccountWorld()
        {
            SlotCount = 3;
            Locker = new ItemLocker(999);
            Trunk = new ItemTrunk(4);
        }
    }
}
