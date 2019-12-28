using System.Threading.Tasks;
using Edelstein.Core.Services.Distributed;
using Edelstein.Entities;
using Edelstein.Network;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrationSocketAdapter : ISocketAdapter
    {
        IMigrationService Service { get; }
        
        Account Account { get; set; }
        AccountWorld AccountWorld { get; set; }
        Character Character { get; set; }

        Task TryMigrateTo(IServerNodeState nodeState);
        Task TryMigrateFrom(int characterID, long clientKey);
    }
}