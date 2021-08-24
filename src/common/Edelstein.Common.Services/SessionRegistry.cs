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
    public class SessionRegistry : ISessionRegistry
    {
        private static readonly string SessionScope = "sessions";
        private static readonly string SessionAccountScope = $"{SessionScope}:account";
        private static readonly string SessionCharacterScope = $"{SessionScope}:character";
        private static readonly string SessionLockScope = $"{SessionScope}:lock";
        private static readonly TimeSpan SessionTimeoutDuration = TimeSpan.FromMinutes(2);
        private static readonly TimeSpan SessionLockTimeoutDuration = TimeSpan.FromSeconds(6);

        private readonly ICacheClient _sessionAccountCache;
        private readonly ICacheClient _sessionCharacterCache;
        private readonly ILockProvider _locker;

        public SessionRegistry(ICacheClient cache, IMessageBus messenger)
        {
            _sessionAccountCache = new ScopedCacheClient(cache, SessionAccountScope);
            _sessionCharacterCache = new ScopedCacheClient(cache, SessionCharacterScope);
            _locker = new CacheLockProvider(new ScopedCacheClient(cache, SessionLockScope), messenger);
        }

        public async Task<StartSessionResponse> Start(StartSessionRequest request)
        {
            var source = new CancellationTokenSource();
            var result = SessionRegistryResult.Ok;

            source.CancelAfter(SessionLockTimeoutDuration);

            try
            {
                var session = request.Session;
                var @lock = await _locker.AcquireAsync(session.Account.ToString(), cancellationToken: source.Token);
                var timeout = DateTime.UtcNow.Add(SessionTimeoutDuration);

                if (await _sessionAccountCache.ExistsAsync(request.Session.Account.ToString()))
                    result = SessionRegistryResult.FailedAlreadyStarted;

                if (result == SessionRegistryResult.Ok)
                {
                    await _sessionAccountCache.SetAsync(session.Account.ToString(), session, timeout);
                    if (session.Character.HasValue)
                        await _sessionCharacterCache.SetAsync(session.Character.Value.ToString(), session, timeout);
                }

                await @lock.ReleaseAsync();
            }
            catch (ThreadInterruptedException)
            {
                result = SessionRegistryResult.FailedTimeout;
            }

            return new StartSessionResponse { Result = result };
        }

        public async Task<UpdateSessionResponse> Update(UpdateSessionRequest request)
        {
            var source = new CancellationTokenSource();
            var result = SessionRegistryResult.Ok;

            source.CancelAfter(SessionLockTimeoutDuration);

            try
            {
                var session = request.Session;
                var @lock = await _locker.AcquireAsync(session.Account.ToString(), cancellationToken: source.Token);
                var timeout = DateTime.UtcNow.Add(SessionTimeoutDuration);

                if (!await _sessionAccountCache.ExistsAsync(request.Session.Account.ToString()))
                    result = SessionRegistryResult.FailedNotStarted;

                if (result == SessionRegistryResult.Ok)
                {
                    await _sessionAccountCache.SetAsync(session.Account.ToString(), session, timeout);
                    if (session.Character.HasValue)
                        await _sessionCharacterCache.SetAsync(session.Character.Value.ToString(), session, timeout);
                }

                await @lock.ReleaseAsync();
            }
            catch (ThreadInterruptedException)
            {
                result = SessionRegistryResult.FailedTimeout;
            }

            return new UpdateSessionResponse { Result = result };
        }

        public async Task<DescribeSessionByAccountResponse> DescribeByAccount(DescribeSessionByAccountRequest request)
        {
            var account = request.Account;
            var session = await _sessionAccountCache.GetAsync<SessionContract>(account.ToString());

            return new DescribeSessionByAccountResponse
            {
                Result = SessionRegistryResult.Ok,
                Session = session.HasValue ? session.Value : new SessionContract
                {
                    Account = account,
                    State = SessionState.Offline
                }
            };
        }

        public async Task<DescribeSessionByCharacterResponse> DescribeByCharacter(DescribeSessionByCharacterRequest request)
        {
            var character = request.Character;
            var session = await _sessionCharacterCache.GetAsync<SessionContract>(character.ToString());

            return new DescribeSessionByCharacterResponse
            {
                Result = SessionRegistryResult.Ok,
                Session = session.HasValue ? session.Value : new SessionContract
                {
                    Character = character,
                    State = SessionState.Offline
                }
            };
        }
    }
}
