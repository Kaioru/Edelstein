using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;

namespace Edelstein.Common.Services
{
    public class MigrationRegistry : IMigrationRegistry
    {
        private static readonly string MigrationScope = "migrations";
        private static readonly string MigrationLockScope = $"{MigrationScope}:lock";
        private static readonly TimeSpan MigrationTimeoutDuration = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan MigrationLockTimeoutDuration = TimeSpan.FromSeconds(6);

        private readonly ICacheClient _cache;
        private readonly ILockProvider _locker;

        public MigrationRegistry(ICacheClient cache, IMessageBus messenger)
        {
            _cache = new ScopedCacheClient(cache, MigrationScope);
            _locker = new CacheLockProvider(new ScopedCacheClient(cache, MigrationLockScope), messenger);
        }

        public async Task<RegisterMigrationResponse> Register(RegisterMigrationRequest request)
        {
            var source = new CancellationTokenSource();
            var result = MigrationRegistryResult.Ok;

            source.CancelAfter(MigrationLockTimeoutDuration);

            try
            {
                var @lock = await _locker.AcquireAsync(request.Migration.Character.ToString(), cancellationToken: source.Token);

                if (await _cache.ExistsAsync(request.Migration.Character.ToString()))
                    result = MigrationRegistryResult.FailedAlreadyRegistered;

                if (result == MigrationRegistryResult.Ok)
                    await _cache.SetAsync(request.Migration.Character.ToString(), request.Migration, MigrationTimeoutDuration);

                await @lock.ReleaseAsync();
            }
            catch (Exception e)
            {
                result = MigrationRegistryResult.FailedTimeout;
            }

            return new RegisterMigrationResponse { Result = result };
        }

        public async Task<DeregisterMigrationResponse> Deregister(DeregisterMigrationRequest request)
        {
            var source = new CancellationTokenSource();
            var result = MigrationRegistryResult.Ok;

            source.CancelAfter(MigrationLockTimeoutDuration);

            try
            {
                var @lock = await _locker.AcquireAsync(request.Character.ToString(), cancellationToken: source.Token);

                if (!await _cache.ExistsAsync(request.Character.ToString()))
                    result = MigrationRegistryResult.FailedNotRegistered;

                if (result == MigrationRegistryResult.Ok)
                    await _cache.RemoveAsync(request.Character.ToString());

                await @lock.ReleaseAsync();
            }
            catch (Exception)
            {
                result = MigrationRegistryResult.FailedTimeout;
            }

            return new DeregisterMigrationResponse { Result = result };
        }

        public async Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(MigrationLockTimeoutDuration);

            try
            {
                var @lock = await _locker.AcquireAsync(request.Character.ToString(), cancellationToken: source.Token);
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

                await @lock.ReleaseAsync();

                return new ClaimMigrationResponse
                {
                    Result = result,
                    Migration = existing.HasValue ? existing.Value : null
                };
            }
            catch (Exception e)
            {
                return new ClaimMigrationResponse { Result = MigrationRegistryResult.FailedTimeout };
            }
        }
    }
}
