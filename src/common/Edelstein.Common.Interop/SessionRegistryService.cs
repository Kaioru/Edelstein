using System;
using System.Threading.Tasks;
using Edelstein.Common.Util.Caching;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Util.Caching;

namespace Edelstein.Common.Interop
{
    public class SessionRegistryService : ISessionRegistryService
    {
        private static readonly string SessionScope = "sessions";
        private static readonly string SessionAccountScope = $"{SessionScope}:account";
        private static readonly string SessionCharacterScope = $"{SessionScope}:character";
        private static readonly TimeSpan SessionTimeoutDuration = TimeSpan.FromMinutes(2);

        private readonly ICache _sessionAccountCache;
        private readonly ICache _sessionCharacterCache;

        public SessionRegistryService(ICache cache)
        {
            _sessionAccountCache = new ScopedCache(SessionAccountScope, cache);
            _sessionCharacterCache = new ScopedCache(SessionCharacterScope, cache);
        }

        public async Task<UpdateSessionResult> UpdateSession(UpdateSessionRequest request)
        {
            var session = request.Session;
            var timeout = DateTime.UtcNow.Add(SessionTimeoutDuration);

            await _sessionAccountCache.Set(session.Account.ToString(), session, timeout);
            if (session.HasCharacter)
                await _sessionCharacterCache.Set(session.Character.ToString(), session, timeout);

            return new UpdateSessionResult { Result = SessionRegistryResult.Ok };
        }

        public async Task<DescribeSessionResult> DescribeSessionByAccount(DescribeSessionByAccountRequest request)
        {
            var account = request.Account;
            var session = await _sessionAccountCache.Get<SessionObject>(account.ToString());

            return new DescribeSessionResult
            {
                Result = SessionRegistryResult.Ok,
                Session = session ?? new SessionObject
                {
                    Account = account,
                    State = SessionState.Offline
                }
            };
        }

        public async Task<DescribeSessionResult> DescribeSessionByCharacter(DescribeSessionByCharacterRequest request)
        {
            var character = request.Character;
            var session = await _sessionCharacterCache.Get<SessionObject>(character.ToString());

            return new DescribeSessionResult
            {
                Result = SessionRegistryResult.Ok,
                Session = session ?? new SessionObject
                {
                    Character = character,
                    State = SessionState.Offline
                }
            };
        }
    }
}
