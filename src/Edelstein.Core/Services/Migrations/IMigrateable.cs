using System.Threading.Tasks;
using Edelstein.Core.Services.Info;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrateable
    {
        Task<bool> TryMigrateTo(int accountID, int characterID, ServiceInfo to);
        Task<bool> TryMigrateFrom(int accountID, int characterID);
    }
}