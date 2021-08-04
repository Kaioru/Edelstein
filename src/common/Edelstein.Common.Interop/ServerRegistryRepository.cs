using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Repositories;

namespace Edelstein.Common.Interop
{
    public class ServerRegistryRepository : LocalRepository<string, ServerRegistryEntry>
    {
        private static readonly TimeSpan ServerTimeoutDuration = TimeSpan.FromMinutes(3);

        public override async Task<ServerRegistryEntry> Retrieve(string key)
        {
            var now = DateTime.UtcNow;
            var result = await base.Retrieve(key);

            if ((now - result.LastUpdate) > ServerTimeoutDuration)
                await Delete(result.ID);

            return await base.Retrieve(key);
        }

        public override Task Delete(string key)
        {
            if (Dictionary.ContainsKey(key))
                Dictionary[key].Dispose();
            return base.Delete(key);
        }
    }
}
