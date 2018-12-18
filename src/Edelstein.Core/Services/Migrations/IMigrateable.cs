using System.Threading.Tasks;
using Edelstein.Core.Services.Info;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrateable
    {
        Task<bool> TryMigrateTo(int id, ServiceInfo to);
        Task<bool> TryMigrateFrom(int id);
    }
}