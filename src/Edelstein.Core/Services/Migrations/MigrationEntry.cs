using Edelstein.Core.Services.Distributed;
using Edelstein.Entities;

namespace Edelstein.Core.Services.Migrations
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