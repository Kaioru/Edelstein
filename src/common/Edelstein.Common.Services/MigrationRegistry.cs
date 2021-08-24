using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Services
{
    public class MigrationRegistry : IMigrationRegistry
    {
        private static readonly string MigrationScope = "migrations";
        private static readonly TimeSpan MigrationTimeoutDuration = TimeSpan.FromMinutes(1);

        private readonly ICache _cache;

        public MigrationRegistry(ICache cache)
            => _cache = new ScopedCache(MigrationScope, cache);

        public async Task<RegisterMigrationResponse> Register(RegisterMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;
            var existing = await _cache.Get<MigrationContract>(request.Migration.Character.ToString());

            if (existing != null) result = MigrationRegistryResult.FailedAlreadyRegistered;

            if (result == MigrationRegistryResult.Ok)
                await _cache.Set(request.Migration.Character.ToString(), request.Migration, MigrationTimeoutDuration);

            return new RegisterMigrationResponse { Result = result };
        }

        public async Task<DeregisterMigrationResponse> Deregister(DeregisterMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;
            var existing = await _cache.Get<MigrationContract>(request.Character.ToString());

            if (existing == null) result = MigrationRegistryResult.FailedNotRegistered;

            if (result == MigrationRegistryResult.Ok)
                await _cache.Remove(request.Character.ToString());

            return new DeregisterMigrationResponse { Result = result };
        }

        public async Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request)
        {
            var result = MigrationRegistryResult.Ok;
            var existing = await _cache.Get<MigrationContract>(request.Character.ToString());

            if (existing == null) result = MigrationRegistryResult.FailedNotRegistered;
            else if (request.Character != existing.Character) result = MigrationRegistryResult.FailedInvalidCharacter;
            else if (request.ClientKey != existing.ClientKey) result = MigrationRegistryResult.FailedInvalidClientKey;
            else if (request.Server != existing.ServerTo) result = MigrationRegistryResult.FailedInvalidServer;

            if (result == MigrationRegistryResult.Ok)
                await _cache.Remove(request.Character.ToString());

            return new ClaimMigrationResponse
            {
                Result = result,
                Migration = existing
            };
        }
    }
}
