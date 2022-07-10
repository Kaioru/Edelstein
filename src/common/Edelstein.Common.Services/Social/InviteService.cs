using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts.Social;
using Edelstein.Protocol.Services.Social;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;

namespace Edelstein.Common.Services.Social
{
    public class InviteService : IInviteService
    {
        private static readonly string InviteScope = "migrations";
        private static readonly string InviteLockScope = $"{InviteScope}:lock";
        private static readonly TimeSpan InviteTimeoutDuration = TimeSpan.FromMinutes(3);
        private static readonly TimeSpan InviteLockTimeoutDuration = TimeSpan.FromSeconds(6);

        private readonly ICacheClient _cache;
        private readonly ILockProvider _locker;

        public InviteService(ICacheClient cache, IMessageBus messenger)
        {
            _cache = new ScopedCacheClient(cache, InviteScope);
            _locker = new CacheLockProvider(new ScopedCacheClient(cache, InviteLockScope), messenger);
        }

        public async Task<InviteRegisterResponse> Register(InviteRegisterRequest request)
        {
            var source = new CancellationTokenSource();
            var result = InviteServiceResult.Ok;

            source.CancelAfter(InviteLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(request.Invite.Invited.ToString(), cancellationToken: source.Token);

            if (@lock != null)
            {
                var key = $"{request.Invite.Invited}:{request.Invite.Type}";

                if (await _cache.ExistsAsync(key))
                    result = InviteServiceResult.FailedAlreadyInvited;

                if (result == InviteServiceResult.Ok)
                    await _cache.SetAsync(key, request.Invite, InviteTimeoutDuration);

                await @lock.ReleaseAsync();
            }
            else result = InviteServiceResult.FailedTimeout;

            return new InviteRegisterResponse { Result = result };
        }

        public async Task<InviteDeregisterResponse> Deregister(InviteDeregisterRequest request)
        {
            var source = new CancellationTokenSource();
            var result = InviteServiceResult.Ok;

            source.CancelAfter(InviteLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(request.Invited.ToString(), cancellationToken: source.Token);

            if (@lock != null)
            {
                var key = $"{request.Invited}:{request.Type}";

                if (!await _cache.ExistsAsync(key))
                    result = InviteServiceResult.FailedNotInvited;

                if (result == InviteServiceResult.Ok)
                    await _cache.RemoveAsync(key);

                await @lock.ReleaseAsync();
            }
            else result = InviteServiceResult.FailedTimeout;

            return new InviteDeregisterResponse { Result = result };
        }

        public async Task<InviteDeregisterAllResponse> DeregisterAll(InviteDeregisterAllRequest request)
        {
            var source = new CancellationTokenSource();
            var result = InviteServiceResult.Ok;

            source.CancelAfter(InviteLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(request.Invited.ToString(), cancellationToken: source.Token);

            if (@lock != null)
            {
                await _cache.RemoveAllAsync(new List<string> {
                    $"{request.Invited}:{InviteType.Unspecified}",
                    $"{request.Invited}:{InviteType.Party}",
                    $"{request.Invited}:{InviteType.PartyApply}"
                });

                await @lock.ReleaseAsync();
            }
            else result = InviteServiceResult.FailedTimeout;

            return new InviteDeregisterAllResponse { Result = result };
        }

        public async Task<InviteClaimResponse> Claim(InviteClaimRequest request)
        {
            var source = new CancellationTokenSource();

            source.CancelAfter(InviteLockTimeoutDuration);

            var @lock = await _locker.AcquireAsync(request.Invited.ToString(), cancellationToken: source.Token);

            if (@lock != null)
            {
                var result = InviteServiceResult.Ok;
                var key = $"{request.Invited}:{request.Type}";
                var existing = await _cache.GetAsync<InviteContract>(key);

                if (!existing.HasValue) result = InviteServiceResult.FailedNotInvited;

                if (result == InviteServiceResult.Ok)
                    await _cache.RemoveAsync(key);

                await @lock.ReleaseAsync();

                return new InviteClaimResponse
                {
                    Result = result,
                    Invite = existing.HasValue ? existing.Value : null
                };
            }

            return new InviteClaimResponse { Result = InviteServiceResult.FailedTimeout };
        }
    }
}
