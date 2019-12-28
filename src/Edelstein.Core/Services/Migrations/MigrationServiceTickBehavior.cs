using System.Threading.Tasks;
using Edelstein.Core.Utils.Ticks;

namespace Edelstein.Core.Services.Migrations
{
    public class MigrationServiceTickBehavior : ITickBehavior
    {
        private readonly IMigrationService _service;

        public MigrationServiceTickBehavior(IMigrationService service)
            => _service = service;

        public Task<bool> TryTick()
        {
            return Task.FromResult(true);
        }
    }
}