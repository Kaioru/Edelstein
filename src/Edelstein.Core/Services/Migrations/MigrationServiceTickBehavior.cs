using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Ticks;

namespace Edelstein.Core.Services.Migrations
{
    public class MigrationServiceTickBehavior : ITickBehavior
    {
        private readonly IMigrationService _service;

        public MigrationServiceTickBehavior(IMigrationService service)
            => _service = service;

        public async Task<bool> TryTick()
        {
            await Task.WhenAll(_service.Sockets.Values.Select(s => s.TrySendHeartbeat()));
            return true;
        }
    }
}