using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Ticks;

namespace Edelstein.Core.Gameplay.Migrations
{
    public class MigrationServiceTickBehavior : ITickBehavior
    {
        private readonly IMigrationService _service;

        public MigrationServiceTickBehavior(IMigrationService service)
            => _service = service;

        public async Task TryTick(DateTime now)
            => await Task.WhenAll(_service.Sockets.Values.Select(s => s.TrySendHeartbeat()));
    }
}