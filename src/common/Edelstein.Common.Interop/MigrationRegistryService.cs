using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Interop
{
    public class MigrationRegistryService : IMigrationRegistryService
    {
        private static readonly string MigrationScope = "migrations";
        private static readonly TimeSpan MigrationTimeoutDuration = TimeSpan.FromMinutes(1);

        private readonly ICache _cache;

        public MigrationRegistryService(ICache cache)
            => _cache = new ScopedCache(MigrationScope, cache);

        public async Task<MigrationRegistryResponse> Register(RegisterMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;
            var migration = request.Migration;

            if ((await _cache.Get<MigrationObject>(migration.Character.ToString())) != null)
                result = MigrationRegistryResult.Failed;
            else await _cache.Set(migration.Character.ToString(), migration, MigrationTimeoutDuration);

            return new MigrationRegistryResponse { Result = result };
        }

        public async Task<MigrationRegistryResponse> Deregister(DeregisterMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;

            if ((await _cache.Get<MigrationObject>(request.Character.ToString())) == null)
                result = MigrationRegistryResult.Failed;
            else await _cache.Remove(request.Character.ToString());

            return new MigrationRegistryResponse { Result = result };
        }

        public async Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;
            var migration = await _cache.Get<MigrationObject>(request.Character.ToString());

            if (migration == null) result = MigrationRegistryResult.Failed;
            else if (request.Server != migration.ToServer) result = MigrationRegistryResult.Failed;
            else if (request.Key != migration.Key) result = MigrationRegistryResult.Failed;

            if (result == MigrationRegistryResult.Ok)
                await _cache.Remove(request.Character.ToString());

            return new ClaimMigrationResponse
            {
                Result = result,
                Migration = migration
            };
        }
    }
}
