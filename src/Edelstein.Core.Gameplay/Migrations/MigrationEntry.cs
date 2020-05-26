using Edelstein.Core.Distributed;
using Edelstein.Core.Entities;
using Edelstein.Core.Entities.Characters;

namespace Edelstein.Core.Gameplay.Migrations
{
    public class MigrationEntry
    {
        public Account Account { get; set; }
        public AccountWorld AccountWorld { get; set; }
        public Character Character { get; set; }

        public long ClientKey { get; set; }

        public IServerNodeState From { get; set; }
        public IServerNodeState To { get; set; }
    }
}