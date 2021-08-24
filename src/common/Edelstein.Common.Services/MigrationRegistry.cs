using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Foundatio.Caching;

namespace Edelstein.Common.Services
{
    public class MigrationRegistry : IMigrationRegistry
    {
        private static readonly string MigrationScope = "migrations";
        private static readonly TimeSpan MigrationTimeoutDuration = TimeSpan.FromMinutes(1);

        private readonly ICacheClient _cache;

        public MigrationRegistry(ICacheClient cache)
            => _cache = new ScopedCacheClient(cache, MigrationScope);

        public async Task<RegisterMigrationResponse> Register(RegisterMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;

            if (await _cache.ExistsAsync(request.Migration.Character.ToString()))
                result = MigrationRegistryResult.FailedAlreadyRegistered;

            if (result == MigrationRegistryResult.Ok)
                await _cache.SetAsync(request.Migration.Character.ToString(), request.Migration, MigrationTimeoutDuration);

            return new RegisterMigrationResponse { Result = result };
        }

        public async Task<DeregisterMigrationResponse> Deregister(DeregisterMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;

            if (!await _cache.ExistsAsync(request.Character.ToString()))
                result = MigrationRegistryResult.FailedNotRegistered;

            if (result == MigrationRegistryResult.Ok)
                await _cache.RemoveAsync(request.Character.ToString());

            return new DeregisterMigrationResponse { Result = result };
        }

        public async Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;
            var existing = await _cache.GetAsync<MigrationContract>(request.Character.ToString());

            if (!existing.HasValue) result = MigrationRegistryResult.FailedNotRegistered;
            else
            {
                if (request.Character != existing.Value.Character) result = MigrationRegistryResult.FailedInvalidCharacter;
                if (request.ClientKey != existing.Value.ClientKey) result = MigrationRegistryResult.FailedInvalidClientKey;
                if (request.Server != existing.Value.ServerTo) result = MigrationRegistryResult.FailedInvalidServer;
            }

            if (result == MigrationRegistryResult.Ok)
                await _cache.RemoveAsync(request.Character.ToString());

            return new ClaimMigrationResponse
            {
                Result = result,
                Migration = existing.HasValue ? existing.Value : null
            };
        }
    }
}
