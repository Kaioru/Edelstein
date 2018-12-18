using System.Threading.Tasks;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities;

namespace Edelstein.Core.Services.Migrations
{
    public interface IMigrateable
    {
        Task<bool> TryMigrateTo(Character character, ServiceInfo to);
        Task<bool> TryMigrateFrom(Character character, ServiceInfo current);
    }
}